using System.Collections.Generic;
using Harmony;

namespace BuildablePOIProps.Chair
{
	public static class ChairPatches
	{
		[HarmonyPatch(typeof(GeneratedBuildings))]
		[HarmonyPatch("LoadGeneratedBuildings")]
		public static class GeneratedBuildings_LoadGeneratedBuildings_Patch
		{
			public static void Prefix()
			{
				Strings.Add($"STRINGS.BUILDINGS.PREFABS.{ChairConfig.Id.ToUpperInvariant()}.NAME", ChairConfig.DisplayName);
				Strings.Add($"STRINGS.BUILDINGS.PREFABS.{ChairConfig.Id.ToUpperInvariant()}.DESC", ChairConfig.Description);
				Strings.Add($"STRINGS.BUILDINGS.PREFABS.{ChairConfig.Id.ToUpperInvariant()}.EFFECT", ChairConfig.Effect);

				ModUtil.AddBuildingToPlanScreen("Furniture", ChairConfig.Id);
			}
		}

		[HarmonyPatch(typeof(Db))]
		[HarmonyPatch("Initialize")]
		public static class Db_Initialize_Patch
		{
			public static void Prefix()
			{
				var luxuryTech = new List<string>(Database.Techs.TECH_GROUPING["Luxury"]) { ChairConfig.Id };
				Database.Techs.TECH_GROUPING["Luxury"] = luxuryTech.ToArray();
			}
		}
	}
}
