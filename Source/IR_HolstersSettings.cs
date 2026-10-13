using Holsters;
using RimWorldHolsters.Utility;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Verse;

namespace RimWorldHolsters
{
    public class IR_HolstersSettings : ModSettings
    {
        public static string LastSelectedHeadType;
        public static bool IsFirstLaunch;    
        public static bool DisplaySidearms;
        public static bool SmartSideDisplay;
        public static bool DisplayIndoors = true;
        public static List<HolsterWeaponRenderGroup> RenderGroups;
        public static List<WeaponGroupCordInfo> groups;

        public IR_HolstersSettings() { }

        public override void ExposeData()
        {
            // New
            Scribe_Collections.Look(ref RenderGroups, "renderGroups", LookMode.Deep);

            // Old
            Scribe_Collections.Look(ref groups, "groupsCordSettings3" /*changed from 3 to 4 for development for now*/, LookMode.Deep);
            Scribe_Values.Look(ref DisplaySidearms, "displaySide", true);
            Scribe_Values.Look(ref SmartSideDisplay, "smartSideDisplay", true);
            Scribe_Values.Look(ref IsFirstLaunch, "isFirstLaunch7", true);

            Scribe_Values.Look(ref DisplayIndoors, "displayIndoors", true);

            base.ExposeData();
        }

        //MANAGING GROUPS
        public static void InitBasicGroups()
        {
            if (ShouldInitializeGroups())
            {
                Log.Message("[Holsters] Groups initialised.");
                ResetAllGroups();
            }
            else
            {
                CheckIfAllWeaponsBelongToAGroup();
            }

            IsFirstLaunch = false;
        }

        public static void RestoreV1WeaponRenderGroups()
        {
            Log.Message("[Holsters] Converting from the old settings.");

            if (groups is null)
            {
                Log.Message("[Holsters] Legacy mod settings are absent. Aborting conversion...");
                return;
            }

            Log.Message("[Holsters] Detected legacy mod settings. Starting conversion...");

            V2RenderGroupTranslator.UpdateModSettings(groups);

            Log.Message("[Holsters] Conversion successful.");
        }

        private static bool ShouldInitializeGroups() => IsFirstLaunch || RenderGroups.NullOrEmpty();


        public static void ResetAllGroups() => RenderGroups = IR_HolstersInitialisation.LoadDefaultWeaponGroups();

        public static void CheckIfAllWeaponsBelongToAGroup()
        {
            var weaponsWithoutGroup = new List<ThingDef>();

            foreach (ThingDef thing in GenDefDatabase.GetAllDefsInDatabaseForDef(typeof(ThingDef)))
            {
                if (GetWeaponGroupOf(thing.defName).Name.Equals("noGroup"))
                {
                    weaponsWithoutGroup.Add(thing);
                }
            }

            IR_HolstersInitialisation.SortWeaponsIntoGroups(ref RenderGroups, weaponsWithoutGroup);
        }

        public static void ResetGroup(HolsterWeaponRenderGroup group) => group.HolsterRenderData.Reset();

        public static void ChangeGroupsName(HolsterWeaponRenderGroup group, string name) => group.Name = name;

        public static void RemoveGroup(HolsterWeaponRenderGroup group) => _ = RenderGroups.Remove(group);

        public static void AddNewSettingsGroup(string name) => RenderGroups.Add(new HolsterWeaponRenderGroup(name));

        public static HolsterWeaponRenderGroup GetWeaponGroupOf(string weaponDefName) =>
            RenderGroups.SingleOrDefault(rg => rg.HasWeapon(weaponDefName)) ?? new HolsterWeaponRenderGroup("noGroup");

        public static Vector3 GetWeaponPos(HolsterWeaponRenderGroup group, Rot4 rot, bool isSide, BodyTypeData body)
        {
            HolsterConfiguration configuration = group.HolsterRenderData.GetConfiguration(rot, isSide);

            Vector3 pos = configuration.Position;

            Vector3 offset = configuration.BodyOffset * group.HolsterRenderData.GetBodyModifier(body.DefName, isSide);

            pos += offset;

            return pos;
        }

        public static Vector3 GetWeaponPos(HolsterWeaponRenderGroup group, Rot4 rot, bool isSide)
        {
            return group.HolsterRenderData.GetConfiguration(rot, isSide).Position;
        }

        public static float GetWeaponAngle(HolsterWeaponRenderGroup group, Rot4 rot, bool isSide) => group.HolsterRenderData.GetConfiguration(rot, isSide).Rotation;

        public static bool GetWeaponFlip(HolsterWeaponRenderGroup group, Rot4 rot, bool isSide) => group.HolsterRenderData.GetConfiguration(rot, isSide).IsFlipped;
    }
}