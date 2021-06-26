using CaiLib.Utils;
using HarmonyLib;
using static CaiLib.Utils.BuildingUtils;
using static CaiLib.Utils.StringUtils;

namespace MarbleTile
{
	public static class MarbleTilePatches
	{
		[HarmonyPatch(typeof(GeneratedBuildings))]
		[HarmonyPatch(nameof(GeneratedBuildings.LoadGeneratedBuildings))]
		public static class GeneratedBuildings_LoadGeneratedBuildings_Patch
		{
			public static void Prefix()
			{
				AddBuildingStrings(MarbleTileConfig.Id, MarbleTileConfig.DisplayName, MarbleTileConfig.Description, MarbleTileConfig.Effect);
				AddBuildingToPlanScreen(GameStrings.PlanMenuCategory.Base, MarbleTileConfig.Id, CarpetTileConfig.ID);
			}
		}

		[HarmonyPatch(typeof(Db))]
		[HarmonyPatch("Initialize")]
		public static class Db_Initialize_Patch
		{
			public static void Postfix()
			{
				AddBuildingToTechnology(GameStrings.Technology.Decor.FineArt, MarbleTileConfig.Id);
			}
		}
	}
}
