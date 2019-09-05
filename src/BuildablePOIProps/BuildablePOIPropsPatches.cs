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
				LogInit();
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
				AddBuildingStrings(LandscapePaintingConfig.Id, LandscapePaintingConfig.DisplayName, LandscapePaintingConfig.Description, LandscapePaintingConfig.Effect);
				AddBuildingStrings(ScienceDegreeConfig.Id, ScienceDegreeConfig.DisplayName, ScienceDegreeConfig.Description, ScienceDegreeConfig.Effect);
				AddBuildingStrings(FakePlantConfig.Id, FakePlantConfig.DisplayName, FakePlantConfig.Description, FakePlantConfig.Effect);
				AddBuildingStrings(WhiteboardConfig.Id, WhiteboardConfig.DisplayName, WhiteboardConfig.Description, WhiteboardConfig.Effect);
				AddBuildingStrings(WindowConfig.Id, WindowConfig.DisplayName, WindowConfig.Description, WindowConfig.Effect);
				AddBuildingStrings(SkeletonDisplayConfig.Id, SkeletonDisplayConfig.DisplayName, SkeletonDisplayConfig.Description, SkeletonDisplayConfig.Effect);
				AddBuildingStrings(BlueprintDisplayAConfig.Id, BlueprintDisplayAConfig.DisplayName, BlueprintDisplayAConfig.Description, BlueprintDisplayAConfig.Effect);
				AddBuildingStrings(BlueprintDisplayBConfig.Id, BlueprintDisplayBConfig.DisplayName, BlueprintDisplayBConfig.Description, BlueprintDisplayBConfig.Effect);
				AddBuildingStrings(BlueprintDisplayCConfig.Id, BlueprintDisplayCConfig.DisplayName, BlueprintDisplayCConfig.Description, BlueprintDisplayCConfig.Effect);
				AddBuildingStrings(GlobeConfig.Id, GlobeConfig.DisplayName, GlobeConfig.Description, GlobeConfig.Effect);
				AddBuildingStrings(DeskConfig.Id, DeskConfig.DisplayName, DeskConfig.Description, DeskConfig.Effect);

				AddBuildingToPlanScreen(GameStrings.PlanMenuCategory.Furniture, ChairConfig.Id);
				AddBuildingToPlanScreen(GameStrings.PlanMenuCategory.Furniture, ClockConfig.Id);
				AddBuildingToPlanScreen(GameStrings.PlanMenuCategory.Furniture, ComputerDeskConfig.Id);
				AddBuildingToPlanScreen(GameStrings.PlanMenuCategory.Furniture, CouchConfig.Id);
				AddBuildingToPlanScreen(GameStrings.PlanMenuCategory.Furniture, DNAStatueConfig.Id);
				AddBuildingToPlanScreen(GameStrings.PlanMenuCategory.Furniture, TableWithChairsConfig.Id);
				AddBuildingToPlanScreen(GameStrings.PlanMenuCategory.Furniture, LandscapePaintingConfig.Id);
				AddBuildingToPlanScreen(GameStrings.PlanMenuCategory.Furniture, ScienceDegreeConfig.Id);
				AddBuildingToPlanScreen(GameStrings.PlanMenuCategory.Furniture, FakePlantConfig.Id);
				AddBuildingToPlanScreen(GameStrings.PlanMenuCategory.Furniture, WhiteboardConfig.Id);
				AddBuildingToPlanScreen(GameStrings.PlanMenuCategory.Furniture, WindowConfig.Id);
				AddBuildingToPlanScreen(GameStrings.PlanMenuCategory.Furniture, SkeletonDisplayConfig.Id);
				AddBuildingToPlanScreen(GameStrings.PlanMenuCategory.Furniture, BlueprintDisplayAConfig.Id);
				AddBuildingToPlanScreen(GameStrings.PlanMenuCategory.Furniture, BlueprintDisplayBConfig.Id);
				AddBuildingToPlanScreen(GameStrings.PlanMenuCategory.Furniture, BlueprintDisplayCConfig.Id);
				AddBuildingToPlanScreen(GameStrings.PlanMenuCategory.Furniture, GlobeConfig.Id);
				AddBuildingToPlanScreen(GameStrings.PlanMenuCategory.Furniture, DeskConfig.Id);
			}
		}

		[HarmonyPatch(typeof(Db))]
		[HarmonyPatch("Initialize")]
		public static class Db_Initialize_Patch
		{
			public static void Prefix()
			{
				AddBuildingToTechnology(GameStrings.Technology.Decor.TextileProduction, ChairConfig.Id);
				AddBuildingToTechnology(GameStrings.Technology.Decor.HomeLuxuries, ClockConfig.Id);
				AddBuildingToTechnology(GameStrings.Technology.Decor.HomeLuxuries, ComputerDeskConfig.Id);
				AddBuildingToTechnology(GameStrings.Technology.Decor.TextileProduction, CouchConfig.Id);
				AddBuildingToTechnology(GameStrings.Technology.Decor.RenaissanceArt, DNAStatueConfig.Id);
				AddBuildingToTechnology(GameStrings.Technology.Decor.HomeLuxuries, TableWithChairsConfig.Id);
				AddBuildingToTechnology(GameStrings.Technology.Decor.HighCulture, LandscapePaintingConfig.Id);
				AddBuildingToTechnology(GameStrings.Technology.Decor.HighCulture, ScienceDegreeConfig.Id);
				AddBuildingToTechnology(GameStrings.Technology.Decor.InteriorDecor, FakePlantConfig.Id);
				AddBuildingToTechnology(GameStrings.Technology.Decor.InteriorDecor, WhiteboardConfig.Id);
				AddBuildingToTechnology(GameStrings.Technology.Decor.GlassBlowing, WindowConfig.Id);
				AddBuildingToTechnology(GameStrings.Technology.Decor.ArtisticExpression, SkeletonDisplayConfig.Id);
				AddBuildingToTechnology(GameStrings.Technology.Decor.HighCulture, BlueprintDisplayAConfig.Id);
				AddBuildingToTechnology(GameStrings.Technology.Decor.HighCulture, BlueprintDisplayBConfig.Id);
				AddBuildingToTechnology(GameStrings.Technology.Decor.HighCulture, BlueprintDisplayCConfig.Id);
				AddBuildingToTechnology(GameStrings.Technology.Decor.RenaissanceArt, GlobeConfig.Id);
				AddBuildingToTechnology(GameStrings.Technology.Decor.HighCulture, DeskConfig.Id);
			}
		}
	}
}