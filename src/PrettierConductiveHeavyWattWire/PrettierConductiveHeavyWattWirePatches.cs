using System;
using Harmony;

namespace PrettierConductiveHeavyWattWire
{
    public static class PrettierConductiveHeavyWattWirePatches
    {
	    public static class Mod_OnLoad
		{
			public static void OnLoad()
		    {
			    CaiLib.Logger.Logger.LogInit(ModInfo.Name, ModInfo.Version);
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
