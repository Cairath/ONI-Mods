using System.Collections.Generic;
using Harmony;

namespace BuildablePOIProps.Clock
{
	public class ClockPatches
	{
		[HarmonyPatch(typeof(GeneratedBuildings))]
		[HarmonyPatch("LoadGeneratedBuildings")]
		public static class GeneratedBuildings_LoadGeneratedBuildings_Patch
		{
			public static void Prefix()
			{
				Strings.Add($"STRINGS.BUILDINGS.PREFABS.{ClockConfig.Id.ToUpperInvariant()}.NAME", ClockConfig.DisplayName);
				Strings.Add($"STRINGS.BUILDINGS.PREFABS.{ClockConfig.Id.ToUpperInvariant()}.DESC", ClockConfig.Description);
				Strings.Add($"STRINGS.BUILDINGS.PREFABS.{ClockConfig.Id.ToUpperInvariant()}.EFFECT", ClockConfig.Effect);

				ModUtil.AddBuildingToPlanScreen("Furniture", ClockConfig.Id);
			}
		}

		[HarmonyPatch(typeof(Db))]
		[HarmonyPatch("Initialize")]
		public static class Db_Initialize_Patch
		{
			public static void Prefix()
			{
				var luxuryTech = new List<string>(Database.Techs.TECH_GROUPING["Luxury"]) { ClockConfig.Id };
				Database.Techs.TECH_GROUPING["Luxury"] = luxuryTech.ToArray();
			}
		}
	}
}
