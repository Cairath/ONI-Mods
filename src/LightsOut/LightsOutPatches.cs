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
				});
			}

			public static void OnLoad()
			{
				LogInit();
			}
		}

		[HarmonyPatch(typeof(MinionIdentity))]
		[HarmonyPatch("Sim1000ms")]
		public static class MinionConfig_Sim1000ms_Patch
		{
			public static void Postfix(MinionIdentity __instance)
			{
				var light = __instance.FindOrAddComponent<Light2D>();

				light.enabled = __instance.GetComponent<SuitEquipper>().IsWearingAirtightSuit();

				//var effects = __instance.FindOrAddComponent<Effects>();
				//effects.Add("PitchBlack", true);
			}
		}

		//[HarmonyPatch(typeof(MinionConfig))]
		//[HarmonyPatch("OnSpawn")]
		//public static class MinionConfig_Patch
		//{
		//	public static void Postfix(MinionIdentity __instance)
		//	{
		//	}
		//}

		[HarmonyPatch(typeof(RationalAi))]
		[HarmonyPatch("InitializeStates")]
		public static class RationalAi_InitializeStates_Patch
		{
			public static void Postfix(RationalAi __instance)
			{
				__instance.alive.ToggleStateMachine(smi => new LightsOutMonitor.Instance(smi.master));
			}
		}

		[HarmonyPatch(typeof(MinionIdentity))]
		[HarmonyPatch("OnSpawn")]
		public static class MinionConfig_Patch
		{
			public static void Postfix(MinionIdentity __instance)
			{
				var light = __instance.FindOrAddComponent<Light2D>();
				light.Color = Color.yellow;
				light.Offset = new Vector2(0f, 1f);
				light.Range = 6;
				light.Lux = 2000;
				light.shape = LightShape.Circle;

				//if (!_configManager.Config.DupeLight) return;

				//var light = go.AddOrGet<Light2D>();
				//light.Range = 2;
				//light.Lux = _configManager.Config.DupeLightLux;
			}
		}

		//[HarmonyPatch(typeof(AtmoSuitConfig))]
		//[HarmonyPatch("DoPostConfigure")]
		//public static class AtmoSuitConfig_DoPostConfigure_Patch
		//{
		//	public static void Postfix(GameObject go)
		//	{

		//	}
		//}

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
					if (codes[i].opcode == OpCodes.Ldc_I4_0 && codes[i+1].opcode == OpCodes.Ble)
					{
						codes[i].opcode = OpCodes.Ldc_I4;
						codes[i].operand = ConfigManager.Config.LitWorkspaceLux;
						break;
					}
				}

				return codes.AsEnumerable();
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

				buttonsList.Insert(buttonsList.Count - 2, new KButtonMenu.ButtonInfo("Toggle Lights Out", Action.NumActions,
					() => { _lightsOut = !_lightsOut; }));

				instance.Field("buttons").SetValue(buttonsList.ToArray());
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

				//CaiLib.Logger.Logger.Log(JsonConvert.SerializeObject(__instance.effects));
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

				for (var y = y0; y <= y1; ++y)
				{
					for (var x = x0; x <= x1; ++x)
					{
						int cell = Grid.XYToCell(x, y);

						if (visible[cell] == 0)
						{
							region.SetBytes(x, y, 0);
							continue;
						}

						var lux = lightIntensityIndexer[cell];
						var luxMapped = Math.Min(lux, config.LuxThreshold);
						var output = Remap(luxMapped, 0, config.LuxThreshold, config.LowestFog, config.HighestFog);

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
