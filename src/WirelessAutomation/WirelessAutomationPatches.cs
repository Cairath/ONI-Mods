using System;
using System.Collections.Generic;
using CaiLib.Utils;
using Harmony;
using static CaiLib.Utils.BuildingUtils;
using static CaiLib.Utils.GameStrings;
using static CaiLib.Utils.StringUtils;

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
				AddBuildingStrings(WirelessSignalEmitterConfig.Id, WirelessSignalEmitterConfig.DisplayName, WirelessSignalEmitterConfig.Description, WirelessSignalEmitterConfig.Effect);
				AddBuildingStrings(WirelessSignalReceiverConfig.Id, WirelessSignalReceiverConfig.DisplayName, WirelessSignalReceiverConfig.Description, WirelessSignalReceiverConfig.Effect);

				Strings.Add(WirelessAutomationManager.SliderTooltipKey, WirelessAutomationManager.SliderTooltip);
				Strings.Add(WirelessAutomationManager.SliderTitleKey, WirelessAutomationManager.SliderTitle);

				AddBuildingToPlanScreen(BuildingMenuCategory.Automation, WirelessSignalEmitterConfig.Id);
				AddBuildingToPlanScreen(BuildingMenuCategory.Automation, WirelessSignalReceiverConfig.Id);
			}

			[HarmonyPatch(typeof(Db))]
			[HarmonyPatch("Initialize")]
			public static class Db_Initialize_Patch
			{
				public static void Prefix()
				{
					AddBuildingToTechnology(GameStrings.Research.Computers.Computing, WirelessSignalEmitterConfig.Id);
					AddBuildingToTechnology(GameStrings.Research.Computers.Computing, WirelessSignalReceiverConfig.Id);
				}
			}
		}
	}
}
