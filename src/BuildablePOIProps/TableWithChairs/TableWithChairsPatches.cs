using System.Collections.Generic;
using Harmony;

namespace BuildablePOIProps.TableWithChairs
{
	public class TableWithChairsPatches
	{
		[HarmonyPatch(typeof(GeneratedBuildings))]
		[HarmonyPatch("LoadGeneratedBuildings")]
		public static class GeneratedBuildings_LoadGeneratedBuildings_Patch
		{
			public static void Prefix()
			{
				Strings.Add($"STRINGS.BUILDINGS.PREFABS.{TableWithChairsConfig.Id.ToUpperInvariant()}.NAME", TableWithChairsConfig.DisplayName);
				Strings.Add($"STRINGS.BUILDINGS.PREFABS.{TableWithChairsConfig.Id.ToUpperInvariant()}.DESC", TableWithChairsConfig.Description);
				Strings.Add($"STRINGS.BUILDINGS.PREFABS.{TableWithChairsConfig.Id.ToUpperInvariant()}.EFFECT", TableWithChairsConfig.Effect);

				ModUtil.AddBuildingToPlanScreen("Furniture", TableWithChairsConfig.Id);
			}
		}

		[HarmonyPatch(typeof(Db))]
		[HarmonyPatch("Initialize")]
		public static class Db_Initialize_Patch
		{
			public static void Prefix()
			{
				var luxuryTech = new List<string>(Database.Techs.TECH_GROUPING["Luxury"]) { TableWithChairsConfig.Id };
				Database.Techs.TECH_GROUPING["Luxury"] = luxuryTech.ToArray();
			}
		}
	}
}
