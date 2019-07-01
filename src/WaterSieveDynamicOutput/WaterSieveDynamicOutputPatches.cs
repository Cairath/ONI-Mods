using Harmony;
using UnityEngine;

namespace WaterSieveDynamicOutput
{
    public static class WaterSieveDynamicOutputPatches
    {
	    [HarmonyPatch(typeof(SplashMessageScreen))]
	    [HarmonyPatch("OnPrefabInit")]
	    public static class SplashMessageScreen_OnPrefabInit_Patch
	    {
		    public static void Postfix()
		    {
			    CaiLib.Logger.LogInit(ModInfo.Name, ModInfo.Version);
		    }
	    }

		[HarmonyPatch(typeof(WaterPurifierConfig))]
	    [HarmonyPatch("ConfigureBuildingTemplate")]
	    public static class WaterPurifierConfig_ConfigureBuildingTemplate_Patch
		{
		    public static void Postfix(WaterPurifierConfig __instance, ref GameObject go)
		    {
				var elementConverter = go.AddOrGet<ElementConverter>();

				elementConverter.outputElements = new[]
			    {
				    new ElementConverter.OutputElement(5f, SimHashes.Water, 0f, true, 0f, 0.5f, true, 0.75f),
				    new ElementConverter.OutputElement(0.2f, SimHashes.ToxicSand, 0f, true, 0f, 0.5f, true, 0.25f)
			    };
			}
	    }
	}
}
