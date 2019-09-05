using System.Reflection;
using CaiLib.Config;
using Harmony;
using static CaiLib.Logger.Logger;

namespace FasterJetpacks
{
	public class FasterJetpacksPatches
	{
		private static ConfigManager<Config> _configManager;

		public static class Mod_OnLoad
		{
			public static void OnLoad()
			{
				LogInit();
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
