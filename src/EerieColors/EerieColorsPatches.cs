using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using HarmonyLib;
using Newtonsoft.Json;
using UnityEngine;

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
					__instance.zoneColours = new Color32[]
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

				UnityEngine.Debug.Log(JsonConvert.SerializeObject(instructions));
				var codes = new List<CodeInstruction>(instructions);
				if (config.UnifiedBiomeBackgrounds)
				{
					for (var i = 0; i < codes.Count; i++)
					{
						if (codes[i].opcode == OpCodes.Beq_S)
						{
							for (var j = i; j < codes.Count; j++)
							{
								if (codes[j].opcode == OpCodes.Stelem_I4)
								{
									UnityEngine.Debug.Log("i executed");
									codes[j-1] = new CodeInstruction(OpCodes.Ldc_I4, config.BiomeBackground);
									break;
								}
							}
						}
					}
				}
				UnityEngine.Debug.Log(JsonConvert.SerializeObject(instructions));

				return codes.AsEnumerable();
			}
		}
	}
}
