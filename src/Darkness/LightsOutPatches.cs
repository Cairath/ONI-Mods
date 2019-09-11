using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Reflection.Emit;
using CaiLib.Config;
using Harmony;
using UnityEngine;
using static CaiLib.Logger.Logger;

namespace LightsOut
{
	public class LightsOutPatches
	{
		private static ConfigManager<Config> _configManager;
		private static bool _lightsOut = true;

		public static class Mod_OnLoad
		{
			public static void PrePatch(HarmonyInstance instance)
			{
				_configManager = new ConfigManager<Config>(ModInfo.Name, Assembly.GetExecutingAssembly().Location);
				_configManager.ReadConfig(() =>
				{
					_configManager.Config.LowestFog = MathUtil.Clamp(0, 255, _configManager.Config.LowestFog);
					_configManager.Config.HighestFog = MathUtil.Clamp(0, 255, _configManager.Config.HighestFog);
					_configManager.Config.LuxThreshold = MathUtil.Clamp(0, int.MaxValue, _configManager.Config.LuxThreshold);
				});
			}

			public static void OnLoad()
			{
				LogInit(ModInfo.Name, ModInfo.Version);
			}
		}

		[HarmonyPatch(typeof(MinionConfig))]
		[HarmonyPatch("OnSpawn")]
		public static class MinionConfig_Patch
		{
			public static void Postfix(GameObject go)
			{
				if (!_configManager.Config.DupeLight) return;

				var light = go.AddOrGet<Light2D>();
				light.Range = 2;
				light.Lux = _configManager.Config.DupeLightLux;
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
						codes[i].operand = _configManager.Config.LitWorkspaceLux;
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

		[HarmonyPatch(typeof(PropertyTextures))]
		[HarmonyPatch("UpdateFogOfWar")]
		public static class EntityConfigManager_LoadGeneratedEntities_Patch
		{
			public static bool Prefix(TextureRegion region, int x0, int y0, int x1, int y1)
			{
				if (!_lightsOut) return true;

				var config = _configManager.Config;
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
