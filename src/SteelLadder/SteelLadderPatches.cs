using System.Collections.Generic;
using Harmony;

namespace SteelLadder
{
	public static class SteelLadderPatches
	{
		[HarmonyPatch(typeof(GeneratedBuildings))]
		[HarmonyPatch("LoadGeneratedBuildings")]
		public static class GeneratedBuildings_LoadGeneratedBuildings_Patch
		{
			private static void Prefix()
			{
				Strings.Add($"STRINGS.BUILDINGS.PREFABS.{SteelLadderConfig.Id.ToUpperInvariant()}.NAME", SteelLadderConfig.DisplayName);
				Strings.Add($"STRINGS.BUILDINGS.PREFABS.{SteelLadderConfig.Id.ToUpperInvariant()}.DESC", SteelLadderConfig.Description);
				Strings.Add($"STRINGS.BUILDINGS.PREFABS.{SteelLadderConfig.Id.ToUpperInvariant()}.EFFECT", SteelLadderConfig.Effect);

				ModUtil.AddBuildingToPlanScreen("Furniture", SteelLadderConfig.Id);
			}
		}

		[HarmonyPatch(typeof(Db), "Initialize")]
		public static class Db_Initialize_Patch
		{
			private static void Prefix()
			{
				var luxuryTech = new List<string>(Database.Techs.TECH_GROUPING["Luxury"]) { SteelLadderConfig.Id };
				Database.Techs.TECH_GROUPING["Luxury"] = luxuryTech.ToArray();
			}
		}
	}
}