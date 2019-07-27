using System.Collections.Generic;
using Harmony;
using TUNING;
using static CaiLib.Logger.Logger;

namespace FancyTile
{
	public static class FancyTilePatches
	{
		public static class Mod_OnLoad
		{
			public static void OnLoad()
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
				Strings.Add($"STRINGS.BUILDINGS.PREFABS.{FancyTileConfig.Id.ToUpperInvariant()}.NAME", FancyTileConfig.DisplayName);
				Strings.Add($"STRINGS.BUILDINGS.PREFABS.{FancyTileConfig.Id.ToUpperInvariant()}.DESC", FancyTileConfig.Description);
				Strings.Add($"STRINGS.BUILDINGS.PREFABS.{FancyTileConfig.Id.ToUpperInvariant()}.EFFECT", FancyTileConfig.Effect);

				AddBuildingToPlanScreen("Base", FancyTileConfig.Id);
			}
		}

		[HarmonyPatch(typeof(Db))]
		[HarmonyPatch("Initialize")]
		public static class Db_Initialize_Patch
		{
			public static void Prefix()
			{
				var luxuryTech = new List<string>(Database.Techs.TECH_GROUPING["Luxury"]) { FancyTileConfig.Id };
				Database.Techs.TECH_GROUPING["Luxury"] = luxuryTech.ToArray();
			}
		}

		private static void AddBuildingToPlanScreen(HashedString category, string buildingId)
		{
			var index = BUILDINGS.PLANORDER.FindIndex(x => x.category == category);

			if (index == -1)
				return;

			var basePlanOrderList = BUILDINGS.PLANORDER[index].data as IList<string>;
			if (basePlanOrderList == null)
			{
				Log(ModInfo.Name, "Could not add Fancy Tile to the building menu.");
				return;
			}

			var carpetIdx = basePlanOrderList.IndexOf(CarpetTileConfig.ID);
			basePlanOrderList.Insert(carpetIdx + 1, buildingId);
		}
	}
}
