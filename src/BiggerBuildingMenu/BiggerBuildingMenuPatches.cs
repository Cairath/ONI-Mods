using CaiLib.Config;
using Harmony;
using static CaiLib.Logger.Logger;

namespace BiggerBuildingMenu
{
	public class BiggerBuildingMenuPatches
	{
		private static ConfigManager<Config> _configManager;

		public static class Mod_OnLoad
		{
			public static void OnLoad()
			{
				LogInit();
				_configManager = new ConfigManager<Config>();
				_configManager.ReadConfig();
			}
		}

		[HarmonyPatch(typeof(PlanScreen))]
		[HarmonyPatch("ConfigurePanelSize")]

		public static class PlanScreen_ConfigurePanelSize_Patch
		{
			public static void Prefix(PlanScreen __instance)
			{
				Traverse.Create(__instance).Field("buildGrid_maxRowsBeforeScroll").SetValue(_configManager.Config.Height);
			}
		}
    }
}
