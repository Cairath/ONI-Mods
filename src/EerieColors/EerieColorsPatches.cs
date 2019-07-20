using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Reflection.Emit;
using CaiLib;
using Harmony;

namespace EerieColors
{
	public static class EerieColorsPatches
	{
		private static ConfigManager<Config> _configManager;

		[HarmonyPatch(typeof(SplashMessageScreen))]
		[HarmonyPatch("OnPrefabInit")]
		public static class SplashMessageScreen_OnPrefabInit_Patch
		{
			public static void Postfix()
			{
				CaiLib.Logger.LogInit(ModInfo.Name, ModInfo.Version);
				_configManager = new ConfigManager<Config>(ModInfo.Name, Assembly.GetExecutingAssembly().Location);
				_configManager.ReadConfig(() =>
				{
					MathUtil.Clamp(_configManager.Config.BiomeBackground, 0, 6);
				});
			}
		}

		[HarmonyPatch(typeof(SubworldZoneRenderData))]
		[HarmonyPatch("GenerateTexture")]
		public static class SubworldZoneRenderData_GenerateTexture_Patch
		{
			public static void Prefix(ref SubworldZoneRenderData __instance)
			{
				var config = _configManager.Config;

				if (config.CustomBiomeTints)
				{
					__instance.zoneColours = new[]
					{
						config.TintColor,
						config.TintColor,
						config.TintColor,
						config.TintColor,
						config.TintColor,
						config.TintColor,
						config.TintColor,
						config.TintColor,
						config.TintColor,
						config.TintColor,
						config.TintColor
					};
				}
			}

			static IEnumerable<CodeInstruction> Transpiler(IEnumerable<CodeInstruction> instructions)
			{
				var config = _configManager.Config;

				var codes = new List<CodeInstruction>(instructions);
				if (config.UnifiedBiomeBackgrounds)
				{
					for (var i = 0; i < codes.Count; i++)
					{
						if (codes[i].opcode == OpCodes.Bne_Un)
						{
							for (var j = i; j < codes.Count; j++)
							{
								if (codes[j].opcode == OpCodes.Stelem_I1)
								{
									codes.Insert(j, new CodeInstruction(OpCodes.Ldc_I4, config.BiomeBackground));
									codes.Insert(j, new CodeInstruction(OpCodes.Pop));
									break;
								}
							}
						}
					}
				}

				return codes.AsEnumerable();
			}
		}
	}
}
