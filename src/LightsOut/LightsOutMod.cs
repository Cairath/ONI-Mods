using CaiLib.Config;
using HarmonyLib;
using KMod;
using static CaiLib.Logger.Logger;

namespace LightsOut
{
	public class LightsOutMod : UserMod2
	{
		public static ConfigManager<Config> ConfigManager;

		public override void OnLoad(Harmony harmony)
		{
			LogInit(mod);

			ConfigManager = new ConfigManager<Config>(mod);
			ConfigManager.ReadConfig(() =>
			{
				ConfigManager.Config.LowestFog = MathUtil.Clamp(0, 255, ConfigManager.Config.LowestFog);
				ConfigManager.Config.HighestFog = MathUtil.Clamp(0, 255, ConfigManager.Config.HighestFog);
				ConfigManager.Config.LuxThreshold = MathUtil.Clamp(0, int.MaxValue, ConfigManager.Config.LuxThreshold);
				ConfigManager.Config.DisturbSleepLux = MathUtil.Clamp(0, int.MaxValue, ConfigManager.Config.DisturbSleepLux);
				ConfigManager.Config.LitWorkspaceLux = MathUtil.Clamp(0, int.MaxValue, ConfigManager.Config.LitWorkspaceLux);
				ConfigManager.Config.LitDecorLux = MathUtil.Clamp(0, int.MaxValue, ConfigManager.Config.LitDecorLux);
				ConfigManager.Config.DebuffTier = (DebuffTier)MathUtil.Clamp(0, 2, (int)ConfigManager.Config.DebuffTier);
			});

			base.OnLoad(harmony);
		}
	}
}