using System.Reflection;
using CaiLib.Config;
using Harmony;
using static CaiLib.Logger.Logger;

namespace FasterJetpacks
{
	public class FasterJetpacksPatches
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

		[HarmonyPatch(typeof(BipedTransitionLayer))]
        [HarmonyPatch(nameof(BipedTransitionLayer.BeginTransition))]
		public static class BipedTransitionLayer_BeginTransition_Patch
		{
			public static void Prefix(ref BipedTransitionLayer __instance)
			{
				var instance = Traverse.Create(__instance);

				var floorSpeed = instance.Field("floorSpeed").GetValue<float>();
				var jetpackSpeed = instance.Field("jetPackSpeed");
				jetpackSpeed.SetValue(floorSpeed * _configManager.Config.SpeedMultiplier);
			}
		}
	}
}
