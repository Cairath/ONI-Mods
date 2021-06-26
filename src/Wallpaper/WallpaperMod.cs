using System.Collections.Generic;
using CaiLib.Config;
using HarmonyLib;
using KMod;
using static CaiLib.Logger.Logger;

namespace Wallpaper
{
	public class WallpaperMod : UserMod2
	{
		internal static ConfigManager<Config> ConfigManager;
		internal static ColorRefresher ColorRefresher;
		private static ConfigWatcher _configWatcher;

		public override void OnLoad(Harmony harmony)
		{
			LogInit(mod);

			ConfigManager = new ConfigManager<Config>(mod);
			ConfigManager.ReadConfig();

			if (ConfigManager.Config.Colors == null)
			{
				ConfigManager.Config.Colors = new Dictionary<string, string>();
			}

			_configWatcher = new ConfigWatcher(OnConfigChanged);

			base.OnLoad(harmony);
		}

		private static void OnConfigChanged()
		{
			ConfigManager.ReadConfig();
			ColorRefresher.MarkDirty();
		}
	}
}