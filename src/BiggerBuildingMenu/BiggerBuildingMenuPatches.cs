using Harmony;

namespace BiggerBuildingMenu
{
	public class BiggerBuildingMenuPatches
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
