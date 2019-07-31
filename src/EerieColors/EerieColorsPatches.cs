using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Reflection.Emit;
using CaiLib.Config;
using Harmony;
using static CaiLib.Logger.Logger;

namespace EerieColors
{
	public class EerieColorsPatches
	{
		private static ConfigManager<Config> _configManager;

		public static class Mod_OnLoad
		{
			public static void OnLoad()
			{
				LogInit(ModInfo.Name, ModInfo.Version);
			}
		}

		[HarmonyPatch(typeof(SubworldZoneRenderData))]
		[HarmonyPatch("GenerateTexture")]
		public static class SubworldZoneRenderData_GenerateTexture_Patch
		{
			public static void Prefix(ref SubworldZoneRenderData __instance)
			{
				var config = _configManager.Config;

				if (config.CustomBiomeTintsEnabled)
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
				_configManager = new ConfigManager<Config>(ModInfo.Name, Assembly.GetExecutingAssembly().Location);
				_configManager.ReadConfig(() => { MathUtil.Clamp(_configManager.Config.BiomeBackground, 0, 6); });

				var config = _configManager?.Config;

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
