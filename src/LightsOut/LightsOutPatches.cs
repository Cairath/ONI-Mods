using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using CaiLib.Config;
using Harmony;
using UnityEngine;
using static CaiLib.Logger.Logger;

namespace LightsOut
{
	public class LightsOutPatches
	{
		public static ConfigManager<Config> ConfigManager;
		private static bool _lightsOut = true;

		public static class Mod_OnLoad
		{
			public static void PrePatch(HarmonyInstance instance)
			{
				ConfigManager = new ConfigManager<Config>();
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
			}

			public static void OnLoad()
			{
				LogInit();
			}
		}

		[HarmonyPatch(typeof(PauseScreen))]
		[HarmonyPatch("OnPrefabInit")]
		public static class PauseScreen_OnPrefabInit_Patch
		{
			public static void Postfix(PauseScreen __instance)
			{
				var instance = Traverse.Create(__instance);
				var buttons = instance.Field("buttons").GetValue<KButtonMenu.ButtonInfo[]>();
				var buttonsList = buttons.ToList();

				buttonsList.Insert(buttonsList.Count - 2, new KButtonMenu.ButtonInfo("Toggle Lights Out",
					Action.NumActions,
					() => _lightsOut = !_lightsOut));

				instance.Field("buttons").SetValue(buttonsList.ToArray());
			}
		}

		[HarmonyPatch(typeof(MinionConfig))]
		[HarmonyPatch("CreatePrefab")]
		public static class MinionConfig_CreatePrefab_Patch
		{
			public static void Postfix(GameObject __result)
			{
				__result.AddOrGet<DupeLights>();
			}
		}

		[HarmonyPatch(typeof(RationalAi))]
		[HarmonyPatch("InitializeStates")]
		public static class RationalAi_InitializeStates_Patch
		{
			public static void Postfix(RationalAi __instance)
			{
				__instance.alive.ToggleStateMachine(smi => new LightsOutMonitor.Instance(smi.master));
			}
		}

		[HarmonyPatch(typeof(DecorProvider))]
		[HarmonyPatch(nameof(DecorProvider.GetLightDecorBonus))]
		public static class DecorProvider_GetLightDecorBonus_Patch
		{
			public static void Postfix(int cell, ref int __result)
			{
				if (Grid.LightIntensity[cell] >= ConfigManager.Config.LitDecorLux)
					__result = TUNING.DECOR.LIT_BONUS;
				__result = 0;
			}
		}

		[HarmonyPatch(typeof(SleepChore))]
		[HarmonyPatch(nameof(SleepChore.IsLightLevelOk))]
		public static class SleepChore_IsLightLevelOk_Patch
		{
			public static void Postfix(int cell, ref bool __result)
			{
				__result = Grid.LightIntensity[cell] <= ConfigManager.Config.DisturbSleepLux;
			}
		}

		[HarmonyPatch(typeof(Workable))]
		[HarmonyPatch(nameof(Workable.GetEfficiencyMultiplier))]
		public static class Workable_GetEfficiencyMultiplier_Patch
		{
			public static IEnumerable<CodeInstruction> Transpiler(IEnumerable<CodeInstruction> instructions)
			{
				var codes = new List<CodeInstruction>(instructions);
				for (var i = 0; i < codes.Count - 1; i++)
				{
					if (codes[i].opcode == OpCodes.Ldc_I4_0 && codes[i + 1].opcode == OpCodes.Ble)
					{
						codes[i].opcode = OpCodes.Ldc_I4;
						codes[i].operand = ConfigManager.Config.LitWorkspaceLux;
						break;
					}
				}

				return codes.AsEnumerable();
			}
		}

		[HarmonyPatch(typeof(ModifierSet))]
		[HarmonyPatch(nameof(ModifierSet.Initialize))]
		public static class ModifierSet_Initialize_Patch
		{
			public static void Postfix(ModifierSet __instance)
			{
				var effects = Effects.GenerateEffectsList(ConfigManager.Config.DebuffTier);

				foreach (var e in effects)
				{
					__instance.effects.Add(e);
				}
			}
		}

		[HarmonyPatch(typeof(PropertyTextures))]
		[HarmonyPatch("UpdateFogOfWar")]
		public static class EntityConfigManager_LoadGeneratedEntities_Patch
		{
			public static bool Prefix(TextureRegion region, int x0, int y0, int x1, int y1)
			{
				if (!_lightsOut) return true;

				var config = ConfigManager.Config;
				byte[] visible = Grid.Visible;
				var lightIntensityIndexer = Grid.LightIntensity;

				var lowestFogVal = config.LowestFog;
				var gameCycle = GameClock.Instance.GetCycle();
				if (lowestFogVal == 0 && gameCycle < 3)
				{
					lowestFogVal = Math.Max(0, (3 - gameCycle) * 10);
				}

				for (var y = y0; y <= y1; ++y)
				{
					for (var x = x0; x <= x1; ++x)
					{
						var cell = Grid.XYToCell(x, y);

						if (visible[cell] == 0)
						{
							region.SetBytes(x, y, 0);
							continue;
						}

						var lux = lightIntensityIndexer[cell];

						if (lux == 0)
						{
							var neighboringCells = new List<int>();

							if (Grid.IsValidCell(Grid.CellAbove(cell))) neighboringCells.Add(Grid.CellAbove(cell));
							if (Grid.IsValidCell(Grid.CellUpRight(cell))) neighboringCells.Add(Grid.CellUpRight(cell));
							if (Grid.IsValidCell(Grid.CellRight(cell))) neighboringCells.Add(Grid.CellRight(cell));
							if (Grid.IsValidCell(Grid.CellDownRight(cell))) neighboringCells.Add(Grid.CellDownRight(cell));
							if (Grid.IsValidCell(Grid.CellBelow(cell))) neighboringCells.Add(Grid.CellBelow(cell));
							if (Grid.IsValidCell(Grid.CellDownLeft(cell))) neighboringCells.Add(Grid.CellDownLeft(cell));
							if (Grid.IsValidCell(Grid.CellLeft(cell))) neighboringCells.Add(Grid.CellLeft(cell));
							if (Grid.IsValidCell(Grid.CellUpLeft(cell))) neighboringCells.Add(Grid.CellUpLeft(cell));

							var light = 0;

							foreach (var c in neighboringCells)
							{
								light += Grid.LightIntensity[c];
							}

							lux = light / neighboringCells.Count;
						}

						var luxMapped = Math.Min(lux, config.LuxThreshold);
						var output = Remap(luxMapped, 0, config.LuxThreshold, lowestFogVal, config.HighestFog);

						region.SetBytes(x, y, (byte)output);
					}
				}

				return false;
			}

			public static float Remap(int value, int from1, int to1, int from2, int to2)
			{
				return (float)(value - from1) / (to1 - from1) * (to2 - from2) + from2;
			}
		}
	}
}

