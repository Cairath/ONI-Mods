using Harmony;

namespace BiggerBuildingMenu
{
    public class BiggerBuildingMenuPatches
    {
	    [HarmonyPatch(typeof(PlanScreen), MethodType.Constructor)]
	    public static class PlanScreen_Patch
		{
		    public static void Postfix(PlanScreen __instance)
		    {
			    Traverse.Create(__instance).Field("buildGrid_maxRowsBeforeScroll").SetValue(8);
		    }
	    }
	}
}
