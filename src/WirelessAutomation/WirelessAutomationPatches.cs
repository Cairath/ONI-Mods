using System;
using System.Collections.Generic;
using Harmony;

namespace WirelessAutomation
{
	public static class WirelessAutomationPatches
	{
		[HarmonyPatch(typeof(SplashMessageScreen))]
		[HarmonyPatch("OnPrefabInit")]
		public static class SplashMessageScreen_OnPrefabInit_Patch
		{
			public static void Postfix()
			{
				CaiLib.Logger.Logger.LogInit(ModInfo.Name, ModInfo.Version);
			}
		}


		[HarmonyPatch(typeof(Game))]
		[HarmonyPatch("OnPrefabInit")]
		public static class Game_OnPrefabInit_Patch
		{
			public static void Postfix(PauseScreen __instance)
			{
				WirelessAutomationManager.ResetEmittersList();
			}
		}

		[HarmonyPatch(typeof(Game))]
		[HarmonyPatch("OnLoadLevel")]
		public static class Game_OnLoadLevel_Patch
		{
			public static void Postfix(PauseScreen __instance)
			{
				WirelessAutomationManager.ResetEmittersList();
			}
		}

		[HarmonyPatch(typeof(GeneratedBuildings))]
		[HarmonyPatch("LoadGeneratedBuildings")]
		public class GeneratedBuildings_LoadGeneratedBuildings_Patch
		{
			public static void Prefix()
			{
				Strings.Add($"STRINGS.BUILDINGS.PREFABS.{WirelessAutomationEmitterConfig.Id.ToUpperInvariant()}.NAME", WirelessAutomationEmitterConfig.DisplayName);
				Strings.Add($"STRINGS.BUILDINGS.PREFABS.{WirelessAutomationEmitterConfig.Id.ToUpperInvariant()}.DESC", WirelessAutomationEmitterConfig.Description);
				Strings.Add($"STRINGS.BUILDINGS.PREFABS.{WirelessAutomationEmitterConfig.Id.ToUpperInvariant()}.EFFECT", WirelessAutomationEmitterConfig.Effect);

				Strings.Add($"STRINGS.BUILDINGS.PREFABS.{WirelessAutomationReceiverConfig.Id.ToUpperInvariant()}.NAME", WirelessAutomationReceiverConfig.DisplayName);
				Strings.Add($"STRINGS.BUILDINGS.PREFABS.{WirelessAutomationReceiverConfig.Id.ToUpperInvariant()}.DESC", WirelessAutomationReceiverConfig.Description);
				Strings.Add($"STRINGS.BUILDINGS.PREFABS.{WirelessAutomationReceiverConfig.Id.ToUpperInvariant()}.EFFECT", WirelessAutomationReceiverConfig.Effect);

				Strings.Add(WirelessAutomationManager.SliderTooltipKey, WirelessAutomationManager.SliderTooltip);
				Strings.Add(WirelessAutomationManager.SliderTitleKey, WirelessAutomationManager.SliderTitle);

				ModUtil.AddBuildingToPlanScreen("Automation", WirelessAutomationEmitterConfig.Id);
				ModUtil.AddBuildingToPlanScreen("Automation", WirelessAutomationReceiverConfig.Id);
			}

			[HarmonyPatch(typeof(Db))]
			[HarmonyPatch("Initialize")]
			public static class Db_Initialize_Patch
			{
				public static void Prefix()
				{
					var tech = new List<string>(Database.Techs.TECH_GROUPING["DupeTrafficControl"]) { WirelessAutomationEmitterConfig.Id, WirelessAutomationReceiverConfig.Id };
					Database.Techs.TECH_GROUPING["DupeTrafficControl"] = tech.ToArray();
				}
			}

			[HarmonyPatch(typeof(KSerialization.Manager))]
			[HarmonyPatch("GetType")]
			[HarmonyPatch(new[] { typeof(string) })]
			public static class KSerializationManager_GetType_Patch
			{
				public static void Postfix(string type_name, ref Type __result)
				{
					if (type_name == "WirelessAutomation.WirelessAutomationReceiver")
					{
						__result = typeof(WirelessAutomationReceiver);
					}

					if (type_name == "WirelessAutomation.WirelessAutomationEmitter")
					{
						__result = typeof(WirelessAutomationEmitter);
					}
				}
			}
		}
	}
}
