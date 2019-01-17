using System;
using Harmony;

namespace PrettierConductiveHeavyWattWire
{
    public class PrettierConductiveHeavyWattWirePatches
    {
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
