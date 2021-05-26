using CaiLib.Utils;
using Harmony;
using static CaiLib.Logger.Logger;
using static CaiLib.Utils.BuildingUtils;
using static CaiLib.Utils.GameStrings;
using static CaiLib.Utils.StringUtils;

namespace WirelessAutomation
{
	public static class WirelessAutomationPatches
	{
		public static class Mod_OnLoad
		{
			public static void OnLoad()
			{
				LogInit();
			}
		}

		[HarmonyPatch(typeof(Game))]
		[HarmonyPatch("OnPrefabInit")]
		public static class Game_OnPrefabInit_Patch
		{
			public static void Postfix(PauseScreen __instance)
			{
				WirelessAutomationManager.ResetEmittersList();
				WirelessAutomationManager.ResetReceiversList();
			}
		}

		[HarmonyPatch(typeof(Game))]
		[HarmonyPatch("OnLoadLevel")]
		public static class Game_OnLoadLevel_Patch
		{
			public static void Postfix(PauseScreen __instance)
			{
				WirelessAutomationManager.ResetEmittersList();
				WirelessAutomationManager.ResetReceiversList();
			}
		}

		[HarmonyPatch(typeof(GeneratedBuildings))]
		[HarmonyPatch(nameof(GeneratedBuildings.LoadGeneratedBuildings))]
		public class GeneratedBuildings_LoadGeneratedBuildings_Patch
		{
			public static void Prefix()
			{
				AddBuildingStrings(WirelessSignalEmitterConfig.Id, WirelessSignalEmitterConfig.DisplayName, WirelessSignalEmitterConfig.Description, WirelessSignalEmitterConfig.Effect);
				AddBuildingStrings(WirelessSignalReceiverConfig.Id, WirelessSignalReceiverConfig.DisplayName, WirelessSignalReceiverConfig.Description, WirelessSignalReceiverConfig.Effect);

				Strings.Add(WirelessAutomationManager.SliderTooltipKey, WirelessAutomationManager.SliderTooltip);
				Strings.Add(WirelessAutomationManager.SliderTitleKey, WirelessAutomationManager.SliderTitle);

				AddBuildingToPlanScreen(PlanMenuCategory.Automation, WirelessSignalEmitterConfig.Id);
				AddBuildingToPlanScreen(PlanMenuCategory.Automation, WirelessSignalReceiverConfig.Id);
			}

			[HarmonyPatch(typeof(Db))]
			[HarmonyPatch("Initialize")]
			public static class Db_Initialize_Patch
			{
				public static void Postfix()
				{
					AddBuildingToTechnology(GameStrings.Technology.Computers.Computing, WirelessSignalEmitterConfig.Id);
					AddBuildingToTechnology(GameStrings.Technology.Computers.Computing, WirelessSignalReceiverConfig.Id);
				}
			}
		}
	}
}
