using System.Collections.Generic;
using HarmonyLib;

namespace RefinedMetalsUsableAsRawMetals
{
    public static class RefinedMetalsUsableAsRawMetalsPatches
    {
        [HarmonyPatch(typeof(ElementLoader))]
        [HarmonyPatch("FinaliseElementsTable")]
        public static class ElementLoader_LoadUserElementData_Patch
        {
            public static void Postfix()
            {
                var copper = ElementLoader.FindElementByHash(SimHashes.Copper);
                var iron = ElementLoader.FindElementByHash(SimHashes.Iron);
                var tungsten = ElementLoader.FindElementByHash(SimHashes.Tungsten);
                var gold = ElementLoader.FindElementByHash(SimHashes.Gold);
                var lead = ElementLoader.FindElementByHash(SimHashes.Lead);
                var aluminum = ElementLoader.FindElementByHash(SimHashes.Aluminum);
              

                var elements = new[] { copper, iron, gold, lead, aluminum, tungsten };

                if (DlcManager.IsExpansion1Active())
                {
	                var cobalt = ElementLoader.FindElementByHash(SimHashes.Cobalt);
	                elements.AddItem(cobalt);
                }

                foreach (var element in elements)
                {
                    var tags = new List<Tag>(element.oreTags) { GameTags.Metal };
                    element.oreTags = tags.ToArray();

                    GameTags.SolidElements.Add(element.tag);
                }

            }
        }
    }
}
