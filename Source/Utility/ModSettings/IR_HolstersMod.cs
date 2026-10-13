using RimWorld;
using RimWorldHolsters.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Verse;

namespace RimWorldHolsters
{
    public class IR_HolstersMod : Mod
    {
        private BodyTypeData _currentBody;

        internal const float PIXEL_RATIO = 100;

        private readonly IR_HolstersSettings _settings;

        private string _errorLog;
        private bool _restoreDefaultConfirmation;

        private float _receivingGroupIndex;
        private string _groupName;

        internal int CurGroupIndex = 0;


        internal Rot4 CurDir = Rot4.South;
        internal List<ThingDef> CurWeapons = new List<ThingDef>();

        internal BodyTypeData CurrentBody 
        { 
            get
            {
                if (_currentBody == null)
                {
                    _currentBody = BodyTypeDataProvider.AllBodyTypes.First();
                }

                return _currentBody;
            }
            set => _currentBody = value;
        }

        internal bool IsSidearmMode = false;
        internal bool IsPrimaryMode = true;

        internal int CurWeaponIndex = 0;

        //private string instructions = "Guide: \nAll placement settings are group specific, not weapon specific. \nIf you use any sidearm mod you can also edit position for those seperately. \nPositions have to be manually adjusted for each side the pawn is looking at. \nBody offsets are there since some bodies have different dimensions. The position offsets are shared for all bodies but can be modified using impacts (impact 0 means body offsets do not affect this body type.)\n";

        public IR_HolstersMod(ModContentPack content) : base(content)
        {
            _settings = GetSettings<IR_HolstersSettings>();
        }

        public override string SettingsCategory() => "Holsters";

        public override void DoSettingsWindowContents(Rect inRect)
        {
            IR_ModSettingsDrawer.Mod = this;

            if (CurWeapons.NullOrEmpty())
            {
                _ = TryLoadWeapons(true);
            } 

            Widgets.DrawTextureFitted(inRect, IR_Textures.background, 1);

            var middleRect = new Rect(inRect.x + (0.3f * inRect.width) +10f, inRect.y, 0.4f * inRect.width -20f, 0.3f * inRect.height);
            var middleListing = new Listing_Standard();
            

            middleListing.Begin(middleRect);

            //middleListing.Label(instructions);

            if (!_errorLog.NullOrEmpty())
            {
                _ = middleListing.Label(_errorLog);
            }
           
            middleListing.End();

            var convertButtonRect = new Rect(inRect.x + (0.2f * inRect.width), inRect.y, 0.2f * inRect.width, 0.1f * inRect.height);

            if (Widgets.ButtonText(convertButtonRect, "Convert from the old settings"))
            {
                IR_HolstersSettings.RestoreV1WeaponRenderGroups();
            }

            var bodyFrameRect = new Rect(inRect.x + (0.2f * inRect.width), inRect.y + (0.34f * inRect.height), 0.6f * inRect.width, 0.6f * inRect.height);
            IR_ModSettingsDrawer.DrawPawn(bodyFrameRect);


            DrawWeaponsManagement(inRect);
            DrawGroupsManagement(inRect);


            //DANGER ZONE
            var dangerZoneRect = new Rect(inRect.x + 10f, inRect.y + (0.55f * inRect.height), 0.2f * inRect.width - 20f, 0.6f * inRect.height);
            Widgets.DrawLineHorizontal(dangerZoneRect.x, dangerZoneRect.y + 24f, dangerZoneRect.width);
            var dangerZoneListing = new Listing_Standard();
            dangerZoneListing.Begin(dangerZoneRect);

            _ = dangerZoneListing.Label("DANGER ZONE");
            dangerZoneListing.Gap();

            if (dangerZoneListing.ButtonText("Restore default settings") && _restoreDefaultConfirmation)
            {
                CurGroupIndex = 0;
                CurWeaponIndex = 0;
                IR_HolstersSettings.ResetAllGroups();

                _ = TryLoadWeapons(true);
                return;
            }

            dangerZoneListing.CheckboxLabeled("Confirm: ", ref _restoreDefaultConfirmation, "It cannot be reversed!");

            dangerZoneListing.End();



            //TODO: Move this perhaps!
            IR_ModSettingsDrawer.DrawBodyManagement(new Rect(inRect.x + (0.8f * inRect.width) + 10f, inRect.y + (0.3f * inRect.height), 0.2f * inRect.width -20f, 0.6f * inRect.height));

            

            //MODE MANAGEMENT
            var leftSideRect = new Rect(inRect.x +10f , inRect.y+ (inRect.height*0.3f), 0.2f * inRect.width - 20f, inRect.height);
            Widgets.DrawLineHorizontal(leftSideRect.x, leftSideRect.y + 24f, leftSideRect.width);


            var leftSideListing = new Listing_Standard();
            leftSideListing.Begin(leftSideRect);

            _ = leftSideListing.Label("Display Settings");
            leftSideListing.Gap();

            string currentModeLabel;
            if (IsSidearmMode)
            {
                currentModeLabel = "sidearms";
            }
            else
            {
                currentModeLabel = "primary";
            }

            leftSideListing.CheckboxLabeled("Display indoors: ", ref IR_HolstersSettings.DisplayIndoors);
            leftSideListing.CheckboxLabeled("Display sidearms: ", ref IR_HolstersSettings.DisplaySidearms);
            leftSideListing.CheckboxLabeled("Smart sidearms: ", ref IR_HolstersSettings.SmartSideDisplay);

            if (leftSideListing.ButtonText("Edit mode: " + currentModeLabel, "Change to edit sidearms positions and primary positions of the weapons."))
            {
                IsSidearmMode = !IsSidearmMode;
                return;
            }

            leftSideListing.End();
        }

        internal ThingDef GetCurWeapon()
        {
            if (CurWeapons.NullOrEmpty())
            {
                return null;
            }

            return CurWeapons[CurWeaponIndex];
        }

        internal HolsterWeaponRenderGroup GetCurGroup()
        {
            if (IR_HolstersSettings.RenderGroups.NullOrEmpty())
            {
                IR_HolstersSettings.InitBasicGroups();
            }

            return IR_HolstersSettings.RenderGroups[CurGroupIndex];
        }

        private void DrawGroupsManagement(Rect rect)
        {
            var mainRect = new Rect(rect.x + (0.7f * rect.width) + 10f, rect.y, 0.3f * rect.width -20f, 0.40f * rect.height);

            Rect gapLine = mainRect;
            //gapLine.x +=15f;

            gapLine.height = 30;

            Widgets.Label(gapLine, "Groups");
            Widgets.DrawLineHorizontal(gapLine.x, gapLine.y + 24f, gapLine.width);

            //SECTION FOR GROUPS

            mainRect.y += 30;

            Rect changeGroupRect = mainRect;
            changeGroupRect.height = 30;

            changeGroupRect.width = mainRect.width * 0.2f;
            if (Widgets.ButtonText(changeGroupRect, "<-"))
            {
                if (CurGroupIndex == 0)
                {
                    CurGroupIndex = IR_HolstersSettings.RenderGroups.Count - 1;
                }
                else
                {
                    CurGroupIndex--;
                }

                _ = TryLoadWeapons(true);
                return;
            }

            changeGroupRect.width = mainRect.width * 0.6f;
            changeGroupRect.x = mainRect.x + (mainRect.width * 0.2f);
            _ = Widgets.ButtonText(changeGroupRect, GetCurGroup().Name, true, false, false);

            changeGroupRect.width = mainRect.width * 0.2f;
            changeGroupRect.x = mainRect.x + (mainRect.width * 0.8f);
            if (Widgets.ButtonText(changeGroupRect, "->"))
            {
                if (CurGroupIndex == IR_HolstersSettings.RenderGroups.Count - 1)
                {
                    CurGroupIndex = 0;
                }
                else
                {
                    CurGroupIndex++;
                }

                 _ = TryLoadWeapons(true);
                return;
            }

            mainRect.y += 30;

            Rect changeNameRect = mainRect;
            changeNameRect.height = 30;

            _groupName = Widgets.TextEntryLabeled(changeNameRect, "Group's name", _groupName);

            changeNameRect.y += 30;
            changeNameRect.width = mainRect.width * 0.5f;

            if (Widgets.ButtonText(changeNameRect, "Change name"))
            {
                if (_groupName.Equals(""))
                {
                    _errorLog = "Name needs at least one letter!";
                    return;
                }

                _errorLog = GetCurGroup().Name + " changed to " + _groupName + ".";
                IR_HolstersSettings.ChangeGroupsName(GetCurGroup(), _groupName);
                _groupName = "";
            }

            changeNameRect.x = mainRect.x + (mainRect.width * 0.5f);

            if (Widgets.ButtonText(changeNameRect, "Create group"))
            {
                if (_groupName.Equals(""))
                {
                    _errorLog = "Name needs at least one letter!";
                    return;
                }

                _errorLog = _groupName + " created!";
                IR_HolstersSettings.AddNewSettingsGroup(_groupName);
            }

            mainRect.y += 60;

            mainRect.height = 30;
            if (Widgets.ButtonText(mainRect ,"Delete group"))
            {
                if (!GetCurGroup().ShouldDisplay)
                {
                    _errorLog = "This group cannot be deleted.";

                } else if (GetCurWeapon() == null)
                {
                    _errorLog = GetCurGroup().Name + " deleted!";
                    IR_HolstersSettings.RemoveGroup(GetCurGroup());
                    
                    CurGroupIndex = 0;
                    return;

                }
                else
                {
                    _errorLog = "The group can be only deleted if it has no weapons assigned (move weapons to another group to resolve that issue)";
                }
            }
        }

        private void DrawWeaponsManagement(Rect rect)
        {
            var mainRect = new Rect(rect.x + 10f, rect.y, 0.3f * rect.width -20f, 0.4f * rect.height);

            Rect gapLine = mainRect;

            gapLine.height = 30;

            Widgets.Label(gapLine, "Weapons");
            Widgets.DrawLineHorizontal(gapLine.x, gapLine.y + 24f, gapLine.width);

            mainRect.y += 30;

            Rect changeWeapons = mainRect;
            changeWeapons.height = 30;

            changeWeapons.width = mainRect.width * 0.2f;
            if (Widgets.ButtonText(changeWeapons, "<-"))
            {
                if (CurWeaponIndex == 0)
                {
                    CurWeaponIndex = CurWeapons.Count - 1;
                }
                else
                {
                    CurWeaponIndex--;
                }
            }

            changeWeapons.width = mainRect.width * 0.6f;
            changeWeapons.x = mainRect.x + (mainRect.width * 0.2f);
            
            if (CurWeapons.NullOrEmpty())
            {
                _ = Widgets.ButtonText(changeWeapons, "Empty group", true, false, false);
            }
            else
            {
                _ = Widgets.ButtonText(changeWeapons, CurWeapons[CurWeaponIndex].label, true, false, false);
            }

            changeWeapons.width = mainRect.width * 0.2f;
            changeWeapons.x = mainRect.x + (mainRect.width * 0.8f);
            if (Widgets.ButtonText(changeWeapons, "->"))
            {
                if (CurWeaponIndex == CurWeapons.Count - 1)
                {
                    CurWeaponIndex = 0;
                }
                else
                {
                    CurWeaponIndex++;
                }
            }

            mainRect.y+=30;
            mainRect.height = 30;

            if (Widgets.ButtonText(mainRect ,"Send to: " + IR_HolstersSettings.RenderGroups[(int)_receivingGroupIndex].Name))
            {
                if (GetCurWeapon() != null)
                {
                    _errorLog = GetCurWeapon().defName + " moved to " + IR_HolstersSettings.RenderGroups[(int)_receivingGroupIndex].Name;
                    ChangeCurWeaponsGroup(GetCurGroup(), (int)_receivingGroupIndex);
                    _ = TryLoadWeapons(false);
                    return;
                }
            }

            mainRect.y+=30;
            _receivingGroupIndex = (int)Widgets.HorizontalSlider(mainRect, _receivingGroupIndex, 0, IR_HolstersSettings.RenderGroups.Count - 1);


            mainRect.y += 30;
        }

        private bool TryLoadWeapons(bool resetIndex)
        {
            if (resetIndex)
            {
                CurWeaponIndex = 0;
            } 
            else
            {
                AdjustWeaponIndex();
            }
            CurWeapons.Clear();

            foreach (ThingDef weapon in GenDefDatabase.GetAllDefsInDatabaseForDef(typeof(ThingDef)))
            {
                if (weapon.IsWeapon && weapon.equipmentType == EquipmentType.Primary && GetCurGroup().Weapons.Contains(weapon.defName) && weapon.tradeability != Tradeability.None)
                {
                    CurWeapons.Add(weapon);
                }
            }
            
            if (CurWeapons.NullOrEmpty())
            {
                return false;
            }
            return true;
        }

        private void AdjustWeaponIndex()
        {
            if (CurWeaponIndex == 0)
                return;
            
            CurWeaponIndex--;
        }

        private void ChangeCurWeaponsGroup(HolsterWeaponRenderGroup fromGroup, int toGroupIndex)
        {
            ThingDef weapon = GetCurWeapon();

            _ = fromGroup.Weapons.Remove(weapon.defName);
            IR_HolstersSettings.RenderGroups[toGroupIndex].Weapons.Add(weapon.defName);
        }
    }
}