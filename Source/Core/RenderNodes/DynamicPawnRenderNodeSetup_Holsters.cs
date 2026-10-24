using System.Collections.Generic;
using Verse;

namespace RimWorldHolsters.Core.RenderNodes
{
    internal class DynamicPawnRenderNodeSetup_Holsters : DynamicPawnRenderNodeSetup
    {
        public override bool HumanlikeOnly => true;

        public override IEnumerable<(PawnRenderNode node, PawnRenderNode parent)> GetDynamicNodes(Pawn pawn, PawnRenderTree tree)
        {
            if (pawn.equipment == null || pawn.equipment.AllEquipmentListForReading.Count == 0)
                yield break;


            var filledSlots = new List<HolsterWeaponRenderGroup>();

            //Log.Message("-----Generating Dynamic Nodes-----");

            PawnRenderNode holsterNode = tree.TryGetNodeByTag(PawnRenderNodeTagDefOf_Holsters.Holster, out PawnRenderNode node) ? node : null;

            if (pawn.equipment.Primary != null && ShouldAddHolsterNode(pawn.equipment.Primary))
            {
                HolsterWeaponRenderGroup curGroup = IR_HolstersSettings.GetWeaponGroupOf(pawn.equipment.Primary.def.defName);
                if (curGroup.ShouldDisplay && !PawnRenderUtility.CarryWeaponOpenly(pawn))
                {
                    foreach ((PawnRenderNode node, PawnRenderNode parent) result in ProcessWeapon(pawn, tree, pawn.equipment.Primary, holsterNode, false, true))
                    {
                        if (result.node != null)
                            yield return result;

                        //Log.Message("Adding main weapon");
                    }
                }

                if (curGroup.ShouldDisplay)
                    filledSlots.Add(curGroup);
            }

            if (!IR_HolstersSettings.DisplaySidearms)
                yield break;

            //Log.Message("Things in inventory: " + pawn.inventory.innerContainer.Count);

            foreach (ThingWithComps item in pawn.inventory.innerContainer)
            {
                if (!ShouldAddHolsterNode(item))
                    continue;

                //Log.Message("Processing: " + item.def.defName);
                HolsterWeaponRenderGroup curGroup = IR_HolstersSettings.GetWeaponGroupOf(item.def.defName);
                if (!curGroup.ShouldDisplay)
                    continue;

                bool isSidearm = IsSide(curGroup, filledSlots);

                foreach ((PawnRenderNode node, PawnRenderNode parent) result in ProcessWeapon(pawn, tree, item, holsterNode, isSidearm, false))
                {
                    if (result.node != null)
                        yield return result;

                    filledSlots.Add(curGroup);
                    //Log.Message("Adding secondary weapon");
                }
            }

            //Log.Message("-----Ending Generating Dynamic Nodes-----");
            //Log.Message(".");
        }
        private static bool ShouldAddHolsterNode(ThingWithComps gear) => gear.def.IsWeapon;

        private bool IsSide(HolsterWeaponRenderGroup curGroup, List<HolsterWeaponRenderGroup> filledSlots)
        {
            bool isSide = true;

            if (IR_HolstersSettings.SmartSideDisplay && !filledSlots.Contains(curGroup))
            {
                isSide = false;
            }

            return isSide;
        }


        private static IEnumerable<(PawnRenderNode node, PawnRenderNode parent)> ProcessWeapon(Pawn pawn, PawnRenderTree tree, ThingWithComps item, PawnRenderNode parentNode, bool isSidearm, bool isMainWeapon)
        {
            PawnRenderNodeProperties pawnRenderNodeProperties = null;
            PawnRenderNode pawnRenderNode2 = null;
            if (parentNode != null)
            {
                if (pawnRenderNode2 == null)
                {
                    pawnRenderNode2 = parentNode;
                }
                pawnRenderNodeProperties = new PawnRenderNodeProperties
                {
                    debugLabel = item.def.defName,
                    workerClass = typeof(PawnRenderNodeWorker_Holsters),
                    baseLayer = pawnRenderNode2.Props.baseLayer,
                    parentTagDef = PawnRenderNodeTagDefOf_Holsters.Holster
                };
            }

            if (tree.ShouldAddNodeToTree(pawnRenderNodeProperties))
            {
                yield return (node: new PawnRenderNode_Holsters(pawn, pawnRenderNodeProperties, tree, item, isSidearm, isMainWeapon), parent: pawnRenderNode2);
            }
            else
            {
                yield return (node: null, parent: pawnRenderNode2);
            }
        }
    }
}
