using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using Harmony;
using UnityEngine;
using UnityStandardAssets.ImageEffects;

namespace EerieColors
{
	public static class EerieColorsPatches
	{
		[HarmonyPatch(typeof(SplashMessageScreen))]
		[HarmonyPatch("OnPrefabInit")]
		public static class SplashMessageScreen_OnPrefabInit_Patch
		{
			public static void Postfix()
			{
				CaiLib.ModCounter.ModCounter.Hit(ModInfo.Name, ModInfo.Version);
				CaiLib.Logger.LogInit(ModInfo.Name, ModInfo.Version);
			}
		}

		[HarmonyPatch(typeof(SubworldZoneRenderData))]
		[HarmonyPatch("GenerateTexture")]
		public static class SubworldZoneRenderData_GenerateTexture_Patch
		{
			public static void Prefix(ref SubworldZoneRenderData __instance)
			{
				if (Config.CustomBiomeTints)
				{
					__instance.zoneColours = new[]
					{
						Config.TintColor,
						Config.TintColor,
						Config.TintColor,
						Config.TintColor,
						Config.TintColor,
						Config.TintColor,
						Config.TintColor,
						Config.TintColor
					};
				}
			}

			static IEnumerable<CodeInstruction> Transpiler(IEnumerable<CodeInstruction> instructions)
			{
				Config.InitConfig();

				var codes = new List<CodeInstruction>(instructions);
				if (Config.UnifiedBiomeBackgrounds)
				{
					for (var i = 0; i < codes.Count; i++)
					{
						if (codes[i].opcode == OpCodes.Bne_Un)
						{
							for (var j = i; j < codes.Count; j++)
							{
								if (codes[j].opcode == OpCodes.Stelem_I1)
								{
									codes.Insert(j, new CodeInstruction(OpCodes.Ldc_I4, Config.BiomeBackground));
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
