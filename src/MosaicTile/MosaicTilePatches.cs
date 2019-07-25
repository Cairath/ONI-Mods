using Harmony;
using static CaiLib.Logger.Logger;
using static CaiLib.Utils.BuildingUtils;

namespace MosaicTile
{
	public static class MosaicTilePatches
	{
		[HarmonyPatch(typeof(SplashMessageScreen))]
		[HarmonyPatch("OnPrefabInit")]
		public static class SplashMessageScreen_OnPrefabInit_Patch
		{
			public static void Postfix()
			{
				LogInit(ModInfo.Name, ModInfo.Version);
			}
		}

		[HarmonyPatch(typeof(GeneratedBuildings))]
		[HarmonyPatch("LoadGeneratedBuildings")]
		public static class GeneratedBuildings_LoadGeneratedBuildings_Patch
		{
			public static void Prefix()
			{
				Strings.Add($"STRINGS.BUILDINGS.PREFABS.{MosaicTileConfig.Id.ToUpperInvariant()}.NAME", MosaicTileConfig.DisplayName);
				Strings.Add($"STRINGS.BUILDINGS.PREFABS.{MosaicTileConfig.Id.ToUpperInvariant()}.DESC", MosaicTileConfig.Description);
				Strings.Add($"STRINGS.BUILDINGS.PREFABS.{MosaicTileConfig.Id.ToUpperInvariant()}.EFFECT", MosaicTileConfig.Effect);

				AddBuildingToPlanScreen("Base", MosaicTileConfig.Id, CarpetTileConfig.ID);
			}
		}

		[HarmonyPatch(typeof(Db))]
		[HarmonyPatch("Initialize")]
		public static class Db_Initialize_Patch
		{
			public static void Prefix()
			{
				AddBuildingToTechnology("Luxury", MosaicTileConfig.Id);
			}
		}
	}
}
