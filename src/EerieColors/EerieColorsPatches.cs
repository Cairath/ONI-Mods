using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using HarmonyLib;

namespace EerieColors
{
	public class EerieColorsPatches
	{
		[HarmonyPatch(typeof(SubworldZoneRenderData))]
		[HarmonyPatch("GenerateTexture")]
		public static class SubworldZoneRenderData_GenerateTexture_Patch
		{
			public static void Prefix(ref SubworldZoneRenderData __instance)
			{
				var config = EerieColorsMod.ConfigManager.Config;

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

			public static IEnumerable<CodeInstruction> Transpiler(IEnumerable<CodeInstruction> instructions)
			{
				var config = EerieColorsMod.ConfigManager.Config;

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
