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
            HolsterWeaponRenderGroup newGroup = new HolsterWeaponRenderGroup(oldGroup.Name);

            newGroup.Weapons = new List<string>(oldGroup.weapons);
            newGroup.ShouldDisplay = oldGroup.isDisplay;

            newGroup.HolsterRenderData = GetRenderData(oldGroup);

            return newGroup;
        }

        private static HolsterRenderData GetRenderData(WeaponGroupCordInfo oldGroup)
        {
            var configurations = new Dictionary<Rot4, HolsterConfiguration>();
            var sideConfigurations = new Dictionary<Rot4, HolsterConfiguration>();

            foreach (Rot4 worldRot in Enum.GetValues(typeof(Rot4)))
            {
                HolsterConfiguration configuration = new HolsterConfiguration
                {
                    Position = oldGroup.pos[worldRot],
                    Rotation = oldGroup.angle[worldRot],
                    Size = oldGroup.GetSize(worldRot),
                    IsAtFront = oldGroup.isFront[worldRot],
                    IsFlipped = oldGroup.flip[worldRot],
                    BodyOffset = oldGroup.bodyOffset[worldRot]
                };

                configurations.Add(worldRot, configuration);

                HolsterConfiguration sideConfiguration = new HolsterConfiguration
                {
                    Position = oldGroup.posSide[worldRot],
                    Rotation = oldGroup.angleSide[worldRot],
                    Size = oldGroup.GetSize(worldRot),
                    IsAtFront = oldGroup.isFrontSide[worldRot],
                    IsFlipped = oldGroup.flipSide[worldRot],
                    BodyOffset = oldGroup.bodyOffsetSide[worldRot]
                };

                sideConfigurations.Add(worldRot, sideConfiguration);
            }

            return new HolsterRenderData
            {
                Configuration = configurations,
                SideConfiguration = sideConfigurations,
                BodyOffsetsModifiers = oldGroup.bodyOffsetModifiers
                
            };
        }
    }
}
