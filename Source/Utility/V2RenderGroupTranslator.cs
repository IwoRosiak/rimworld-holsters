using Holsters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Verse;

namespace RimWorldHolsters.Utility
{
    internal static class V2RenderGroupTranslator
    {
        internal static List<HolsterWeaponRenderGroup> UpdateModSettings(IReadOnlyCollection<WeaponGroupCordInfo> oldRenderGroups)
        {
            var newRenderGroups = new List<HolsterWeaponRenderGroup>();

            foreach (WeaponGroupCordInfo oldGroup in oldRenderGroups)
            {
                HolsterWeaponRenderGroup newRenderGroup = TranslateWeaponGroup(oldGroup);
                newRenderGroups.Add(newRenderGroup);
            }

            return newRenderGroups;
        }
         
        private static HolsterWeaponRenderGroup TranslateWeaponGroup(WeaponGroupCordInfo oldGroup)
        {
            HolsterWeaponRenderGroup newGroup = new HolsterWeaponRenderGroup(oldGroup.Name)
            {
                Weapons = new List<string>(oldGroup.weapons),
                ShouldDisplay = oldGroup.isDisplay,

                HolsterRenderData = GetRenderData(oldGroup)
            };

            return newGroup;
        }

        private static HolsterRenderData GetRenderData(WeaponGroupCordInfo oldGroup)
        {
            var configurations = new Dictionary<Rot4, HolsterConfiguration>();
            var sideConfigurations = new Dictionary<Rot4, HolsterConfiguration>();
           
            foreach (Rot4 worldRot in Rot4.AllRotations)
            {
                HolsterConfiguration configuration = new HolsterConfiguration(
                    position: oldGroup.pos[worldRot],
                    rotation: oldGroup.angle[worldRot],
                    size: oldGroup.GetSize(worldRot),
                    isFlipped: oldGroup.flip[worldRot],
                    isAtFront: oldGroup.isFront[worldRot],
                    bodyOffset: oldGroup.bodyOffset[worldRot]
                );

                configurations.Add(worldRot, configuration);

                HolsterConfiguration sideConfiguration = new HolsterConfiguration(
                    position: oldGroup.posSide[worldRot],
                    rotation: oldGroup.angleSide[worldRot],
                    size: oldGroup.GetSize(worldRot),
                    isFlipped: oldGroup.flipSide[worldRot],
                    isAtFront: oldGroup.isFrontSide[worldRot],
                    bodyOffset: oldGroup.bodyOffsetSide[worldRot]
                );

                sideConfigurations.Add(worldRot, sideConfiguration);
            }

            return new HolsterRenderData
            {
                Configuration = configurations,
                SideConfiguration = sideConfigurations,
                BodyOffsetsModifiers = oldGroup.bodyOffsetModifs.ToDictionary(modif => modif.Key.ToString(), modif => modif.Value),
                BodyOffsetsSideModifiers = oldGroup.bodyOffsetModifsSide.ToDictionary(modif => modif.Key.ToString(), modif => modif.Value),
            };
        }
    }
}
