using HarmonyLib;

namespace BiggerBuildingMenu
{
	public class BiggerBuildingMenuPatches
	{
		[HarmonyPatch(typeof(PlanScreen))]
		[HarmonyPatch("ConfigurePanelSize")]

		public static class PlanScreen_ConfigurePanelSize_Patch
		{
			public static void Prefix(PlanScreen __instance)
			{
				Traverse.Create(__instance).Field("buildGrid_maxRowsBeforeScroll").SetValue(BiggerBuildingMenuMod.ConfigManager.Config.Height);
			}
		}
    }
}
