using CaiLib.Config;
using HarmonyLib;
using KMod;
using static CaiLib.Logger.Logger;

namespace FasterJetpacks
{
	public class FasterJetpacksMod : UserMod2
	{
		internal static ConfigManager<Config> ConfigManager;

		public override void OnLoad(Harmony harmony)
		{
			LogInit(mod);

			ConfigManager = new ConfigManager<Config>(mod);
			ConfigManager.ReadConfig();

			base.OnLoad(harmony);
		}
	}
}