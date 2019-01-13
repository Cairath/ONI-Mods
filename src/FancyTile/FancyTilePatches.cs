using System.Collections.Generic;
using Harmony;
using TUNING;

namespace FancyTile
{
	public static class FancyTilePatches
	{
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

			(BUILDINGS.PLANORDER[index].data as IList<string>)?.Add(buildingId);
		}
	}
}
