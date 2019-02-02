using System;
using Harmony;

namespace PrettierConductiveHeavyWattWire
{
    public static class PrettierConductiveHeavyWattWirePatches
    {
	    [HarmonyPatch(typeof(SplashMessageScreen))]
	    [HarmonyPatch("OnPrefabInit")]
	    public static class SplashMessageScreen_OnPrefabInit_Patch
	    {
		    public static void Postfix()
		    {
			    CaiLib.ModCounter.ModCounter.Hit(ModInfo.Name, ModInfo.Version);
			    CaiLib.Logger.LogInit(ModInfo.Name, ModInfo.Version);
		    }
	    }

		[HarmonyPatch(typeof(WireRefinedHighWattageConfig))]
	    [HarmonyPatch("CreateBuildingDef")]
	    [HarmonyPatch(new Type[]{})]
	    public static class WireRefinedHighWattageConfig_CreateBuildingDef_Patch
		{
		    public static void Postfix(ref BuildingDef __result)
		    {
			    __result.BaseDecor = -5f;
			    __result.BaseDecorRadius = 3;
		    }
	    }
	}
}
