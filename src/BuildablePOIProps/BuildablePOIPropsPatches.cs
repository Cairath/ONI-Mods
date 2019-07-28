using CaiLib.Utils;
using Harmony;
using static CaiLib.Logger.Logger;
using static CaiLib.Utils.BuildingUtils;
using static CaiLib.Utils.StringUtils;

namespace BuildablePOIProps
{
	public static class BuildablePOIPropsPatches
	{
		public static class Mod_OnLoad
		{
			public static void OnLoad()
			{
				LogInit(ModInfo.Name, ModInfo.Version);
			}
		}

		[HarmonyPatch(typeof(GeneratedBuildings))]
		[HarmonyPatch(nameof(GeneratedBuildings.LoadGeneratedBuildings))]
		public static class GeneratedBuildings_LoadGeneratedBuildings_Patch
		{
			public static void Prefix()
			{
				AddBuildingStrings(ChairConfig.Id, ChairConfig.DisplayName, ChairConfig.Description, ChairConfig.Effect);
				AddBuildingStrings(ClockConfig.Id, ClockConfig.DisplayName, ClockConfig.Description, ClockConfig.Effect);
				AddBuildingStrings(ComputerDeskConfig.Id, ComputerDeskConfig.DisplayName, ComputerDeskConfig.Description, ComputerDeskConfig.Effect);
				AddBuildingStrings(CouchConfig.Id, CouchConfig.DisplayName, CouchConfig.Description, CouchConfig.Effect);
				AddBuildingStrings(DNAStatueConfig.Id, DNAStatueConfig.DisplayName, DNAStatueConfig.Description, DNAStatueConfig.Effect);
				AddBuildingStrings(TableWithChairsConfig.Id, TableWithChairsConfig.DisplayName, TableWithChairsConfig.Description, TableWithChairsConfig.Effect);

				AddBuildingToPlanScreen(GameStrings.BuildingMenuCategory.Furniture, ChairConfig.Id);
				AddBuildingToPlanScreen(GameStrings.BuildingMenuCategory.Furniture, ClockConfig.Id);
				AddBuildingToPlanScreen(GameStrings.BuildingMenuCategory.Furniture, ComputerDeskConfig.Id);
				AddBuildingToPlanScreen(GameStrings.BuildingMenuCategory.Furniture, CouchConfig.Id);
				AddBuildingToPlanScreen(GameStrings.BuildingMenuCategory.Furniture, DNAStatueConfig.Id);
				AddBuildingToPlanScreen(GameStrings.BuildingMenuCategory.Furniture, TableWithChairsConfig.Id);
			}
		}

		[HarmonyPatch(typeof(Db))]
		[HarmonyPatch("Initialize")]
		public static class Db_Initialize_Patch
		{
			public static void Prefix()
			{
				AddBuildingToTechnology(GameStrings.Research.Decor.TextileProduction, ChairConfig.Id);
				AddBuildingToTechnology(GameStrings.Research.Decor.HomeLuxuries, ClockConfig.Id);
				AddBuildingToTechnology(GameStrings.Research.Decor.HomeLuxuries, ComputerDeskConfig.Id);
				AddBuildingToTechnology(GameStrings.Research.Decor.TextileProduction, CouchConfig.Id);
				AddBuildingToTechnology(GameStrings.Research.Decor.RenaissanceArt, DNAStatueConfig.Id);
				AddBuildingToTechnology(GameStrings.Research.Decor.HomeLuxuries, TableWithChairsConfig.Id);
			}
		}
	}
}