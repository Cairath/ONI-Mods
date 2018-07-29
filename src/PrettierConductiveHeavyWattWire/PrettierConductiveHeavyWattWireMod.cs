using Harmony;

namespace PrettierConductiveHeavyWattWire
{
    public class PrettierConductiveHeavyWattWireMod
    {
	    [HarmonyPatch(typeof(WireRefinedHighWattageConfig), "CreateBuildingDef")]
	    public static class PrettierConductiveHeavyWattWirePatch
		{
		    public static void Postfix(ref BuildingDef __result)
		    {
			    __result.BaseDecor = -5f;
			    __result.BaseDecorRadius = 3;
		    }
	    }
	}
}
