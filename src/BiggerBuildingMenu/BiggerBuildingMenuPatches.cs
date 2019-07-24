using System.Reflection;
using CaiLib;
using CaiLib.Config;
using Harmony;
using static CaiLib.Logger.Logger;

namespace BiggerBuildingMenu
{
	public class BiggerBuildingMenuPatches
	{
		private static ConfigManager<Config> _configManager;

		[HarmonyPatch(typeof(SplashMessageScreen))]
		[HarmonyPatch("OnPrefabInit")]
		public static class SplashMessageScreen_OnPrefabInit_Patch
		{
			public static void Postfix()
			{
				LogInit(ModInfo.Name, ModInfo.Version);
				_configManager = new ConfigManager<Config>(ModInfo.Name, Assembly.GetExecutingAssembly().Location);
				_configManager.ReadConfig();
			}
		}

		[HarmonyPatch(typeof(PlanScreen), MethodType.Constructor)]
		public static class PlanScreen_Patch
		{
			public static void Postfix(PlanScreen __instance)
			{
				Traverse.Create(__instance).Field("buildGrid_maxRowsBeforeScroll").SetValue(_configManager.Config.Height);
			}
		}
	}
}
