using System.Collections.Generic;
using Harmony;

namespace BuildablePOIProps.Couch
{
	public class CouchPatches
	{
		[HarmonyPatch(typeof(GeneratedBuildings))]
		[HarmonyPatch("LoadGeneratedBuildings")]
		public static class GeneratedBuildings_LoadGeneratedBuildings_Patch
		{
			public static void Prefix()
			{
				Strings.Add($"STRINGS.BUILDINGS.PREFABS.{CouchConfig.Id.ToUpperInvariant()}.NAME", CouchConfig.DisplayName);
				Strings.Add($"STRINGS.BUILDINGS.PREFABS.{CouchConfig.Id.ToUpperInvariant()}.DESC", CouchConfig.Description);
				Strings.Add($"STRINGS.BUILDINGS.PREFABS.{CouchConfig.Id.ToUpperInvariant()}.EFFECT", CouchConfig.Effect);

				ModUtil.AddBuildingToPlanScreen("Furniture", CouchConfig.Id);
			}
		}

		[HarmonyPatch(typeof(Db))]
		[HarmonyPatch("Initialize")]
		public static class Db_Initialize_Patch
		{
			public static void Prefix()
			{
				var luxuryTech = new List<string>(Database.Techs.TECH_GROUPING["Luxury"]) { CouchConfig.Id };
				Database.Techs.TECH_GROUPING["Luxury"] = luxuryTech.ToArray();
			}
		}
	}
}
