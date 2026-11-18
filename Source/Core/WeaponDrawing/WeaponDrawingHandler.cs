using System;
using System.Collections.Generic;
using UnityEngine;
using Verse;

namespace RimWorldHolsters.Core
{
    internal sealed class WeaponDrawingHandler
    {
        private readonly Pawn _pawn;
        private readonly List<WeaponGroupCordInfo> _filledSlots = new List<WeaponGroupCordInfo>();
        private readonly Vector3 _rootLoc;
        private readonly Rot4 _pawnRotation;

        internal WeaponDrawingHandler(Pawn pawn, Vector3 rootLoc, Rot4 pawnRotation)
        {
            _pawn = pawn;
            _rootLoc = rootLoc;
            _pawnRotation = pawnRotation;
        }

        public void DrawEquipment(bool isCarryingWeapon)
        {

            if (!isCarryingWeapon)
            {
                DrawWeapon(_pawn.equipment.Primary);
            }
            else
            {
                WeaponGroupCordInfo curGroup = IR_HolstersSettings.GetWeaponGroupOf(_pawn.equipment.Primary.def.defName);
                _filledSlots.Add(curGroup);
            }

            if (!IR_HolstersSettings.displaySide)
                return;

            foreach (Thing thing in _pawn.inventory.innerContainer)
            {
                if (!thing.def.IsWeapon) 
                    continue;

                DrawWeapon((ThingWithComps)thing);
            }
        }

        private void DrawWeapon(ThingWithComps weapon)
        {
            WeaponGroupCordInfo curGroup = IR_HolstersSettings.GetWeaponGroupOf(weapon.def.defName);

            if (!curGroup.isDisplay)
                return;

            bool isSide = IsSide(curGroup);

            Vector3 positionOnPawn;
            try
            {
                positionOnPawn = IR_HolstersSettings.GetWeaponPos(weapon.def.defName, _pawnRotation, isSide, _pawn, curGroup);
            }
            catch (ArgumentNullException)
            {
                return; // No body definition
            }

            positionOnPawn += _rootLoc;

            var drawingProperties = new WeaponDrawingProperties(weapon, curGroup, positionOnPawn, _pawnRotation, isSide);

            IR_DisplayWeapon.DrawEquipmentHolstered(drawingProperties);

            _filledSlots.Add(curGroup);
        }

        private bool IsSide(WeaponGroupCordInfo curGroup)
        {
            return IR_HolstersSettings.smartSideDisplay && _filledSlots.Contains(curGroup);
        }
    }
}

