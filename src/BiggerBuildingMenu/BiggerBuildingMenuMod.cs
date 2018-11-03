using Harmony;

namespace BiggerBuildingMenu
{
    public class BiggerBuildingMenuMod
    {
	    [HarmonyPatch(typeof(PlanScreen), MethodType.Constructor)]
	    public static class PlanScreenPatch
		{
		    public static void Postfix(PlanScreen __instance)
		    {
			    var ps = Traverse.Create(__instance).Field("buildGrid_maxRowsBeforeScroll").SetValue(6);
		    }
	    }
	}
}
