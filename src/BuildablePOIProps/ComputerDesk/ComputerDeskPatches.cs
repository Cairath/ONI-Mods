using System.Collections.Generic;
using Harmony;

namespace BuildablePOIProps.ComputerDesk
{
	public class ComputerDeskPatches
	{
		[HarmonyPatch(typeof(GeneratedBuildings))]
		[HarmonyPatch("LoadGeneratedBuildings")]
		public static class GeneratedBuildings_LoadGeneratedBuildings_Patch
		{
			public static void Prefix()
			{
				Strings.Add($"STRINGS.BUILDINGS.PREFABS.{ComputerDeskConfig.Id.ToUpperInvariant()}.NAME", ComputerDeskConfig.DisplayName);
				Strings.Add($"STRINGS.BUILDINGS.PREFABS.{ComputerDeskConfig.Id.ToUpperInvariant()}.DESC", ComputerDeskConfig.Description);
				Strings.Add($"STRINGS.BUILDINGS.PREFABS.{ComputerDeskConfig.Id.ToUpperInvariant()}.EFFECT", ComputerDeskConfig.Effect);

				ModUtil.AddBuildingToPlanScreen("Furniture", ComputerDeskConfig.Id);
			}
		}

		[HarmonyPatch(typeof(Db))]
		[HarmonyPatch("Initialize")]
		public static class Db_Initialize_Patch
		{
			public static void Prefix()
			{
				var luxuryTech = new List<string>(Database.Techs.TECH_GROUPING["Luxury"]) { ComputerDeskConfig.Id };
				Database.Techs.TECH_GROUPING["Luxury"] = luxuryTech.ToArray();
			}
		}
	}
}
