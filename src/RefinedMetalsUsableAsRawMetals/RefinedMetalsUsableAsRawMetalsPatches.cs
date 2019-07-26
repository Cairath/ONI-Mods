using System.Collections.Generic;
using Harmony;
using static CaiLib.Logger.Logger;

namespace RefinedMetalsUsableAsRawMetals
{
    public static class RefinedMetalsUsableAsRawMetalsPatches
    {
        [HarmonyPatch(typeof(SplashMessageScreen))]
        [HarmonyPatch("OnPrefabInit")]
        public static class SplashMessageScreen_OnPrefabInit_Patch
        {
            public static void Postfix()
            {
                LogInit(ModInfo.Name, ModInfo.Version);
            }
        }

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
