using System.Collections.Generic;
using Harmony;

namespace MosaicTile
{
	public static class MosaicTilePatches
	{
		[HarmonyPatch(typeof(GeneratedBuildings))]
		[HarmonyPatch("LoadGeneratedBuildings")]
		public static class GeneratedBuildings_LoadGeneratedBuildings_Patch
		{
			public static void Prefix()
			{
				Strings.Add($"STRINGS.BUILDINGS.PREFABS.{MosaicTileConfig.Id.ToUpperInvariant()}.NAME", MosaicTileConfig.DisplayName);
				Strings.Add($"STRINGS.BUILDINGS.PREFABS.{MosaicTileConfig.Id.ToUpperInvariant()}.DESC", MosaicTileConfig.Description);
				Strings.Add($"STRINGS.BUILDINGS.PREFABS.{MosaicTileConfig.Id.ToUpperInvariant()}.EFFECT", MosaicTileConfig.Effect);

				ModUtil.AddBuildingToPlanScreen("Furniture", MosaicTileConfig.Id);
			}
		}

		[HarmonyPatch(typeof(Db))]
		[HarmonyPatch("Initialize")]
		public static class Db_Initialize_Patch
		{
			public static void Prefix()
			{
				var luxuryTech = new List<string>(Database.Techs.TECH_GROUPING["Luxury"]) { MosaicTileConfig.Id };
				Database.Techs.TECH_GROUPING["Luxury"] = luxuryTech.ToArray();
			}
		}
	}
}
