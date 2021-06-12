using CaiLib.Utils;
using HarmonyLib;
using static CaiLib.Logger.Logger;
using static CaiLib.Utils.BuildingUtils;
using static CaiLib.Utils.StringUtils;

namespace MosaicTile
{
	public static class MosaicTilePatches
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
				AddBuildingStrings(MosaicTileConfig.Id, MosaicTileConfig.DisplayName, MosaicTileConfig.Description, MosaicTileConfig.Effect);
				AddBuildingToPlanScreen("Base", MosaicTileConfig.Id, CarpetTileConfig.ID);
			}
		}

		[HarmonyPatch(typeof(Db))]
		[HarmonyPatch("Initialize")]
		public static class Db_Initialize_Patch
		{
			public static void Postfix()
			{
				AddBuildingToTechnology(GameStrings.Technology.Decor.HighCulture, MosaicTileConfig.Id);
			}
		}
	}
}
