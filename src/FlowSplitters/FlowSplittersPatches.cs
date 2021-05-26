using CaiLib.Utils;
using Harmony;
using static CaiLib.Logger.Logger;
using static CaiLib.Utils.BuildingUtils;
using static CaiLib.Utils.StringUtils;

namespace FlowSplitters
{
	public class FlowSplittersPatches
	{
		public static class Mod_OnLoad
		{
			public static void OnLoad()
			{
				LogInit();
			}
		}

		[HarmonyPatch(typeof(GeneratedBuildings))]
		[HarmonyPatch(nameof(GeneratedBuildings.LoadGeneratedBuildings))]
		public class GeneratedBuildings_LoadGeneratedBuildings_Patch
		{
			public static void Prefix()
			{
				AddBuildingStrings(LiquidSplitterAConfig.Id, LiquidSplitterAConfig.DisplayName, LiquidSplitterAConfig.Description, LiquidSplitterAConfig.Effect);
				AddBuildingStrings(LiquidSplitterBConfig.Id, LiquidSplitterBConfig.DisplayName, LiquidSplitterBConfig.Description, LiquidSplitterBConfig.Effect);
				AddBuildingToPlanScreen(GameStrings.PlanMenuCategory.Plumbing, LiquidSplitterAConfig.Id);
				AddBuildingToPlanScreen(GameStrings.PlanMenuCategory.Plumbing, LiquidSplitterBConfig.Id);

				AddBuildingStrings(GasSplitterAConfig.Id, GasSplitterAConfig.DisplayName, GasSplitterAConfig.Description, GasSplitterAConfig.Effect);
				AddBuildingStrings(GasSplitterBConfig.Id, GasSplitterBConfig.DisplayName, GasSplitterBConfig.Description, GasSplitterBConfig.Effect);
				AddBuildingToPlanScreen(GameStrings.PlanMenuCategory.Ventilation, GasSplitterAConfig.Id);
				AddBuildingToPlanScreen(GameStrings.PlanMenuCategory.Ventilation, GasSplitterBConfig.Id);
			}
		}

		[HarmonyPatch(typeof(Db))]
		[HarmonyPatch("Initialize")]
		public class Db_Initialize_Patch
		{
			public static void Postfix()
			{
				AddBuildingToTechnology(GameStrings.Technology.Gases.Ventilation, GasSplitterAConfig.Id);
				AddBuildingToTechnology(GameStrings.Technology.Gases.Ventilation, GasSplitterBConfig.Id);

				AddBuildingToTechnology(GameStrings.Technology.Liquids.Plumbing, LiquidSplitterAConfig.Id);
				AddBuildingToTechnology(GameStrings.Technology.Liquids.Plumbing, LiquidSplitterBConfig.Id);
			}
		}
	}
}
