using System.Collections.Generic;
using Harmony;

namespace BuildablePOIProps.DNAStatue
{
	public class DNAStatuePatches
	{
		[HarmonyPatch(typeof(GeneratedBuildings))]
		[HarmonyPatch("LoadGeneratedBuildings")]
		public static class GeneratedBuildings_LoadGeneratedBuildings_Patch
		{
			public static void Prefix()
			{
				Strings.Add($"STRINGS.BUILDINGS.PREFABS.{DNAStatueConfig.Id.ToUpperInvariant()}.NAME", DNAStatueConfig.DisplayName);
				Strings.Add($"STRINGS.BUILDINGS.PREFABS.{DNAStatueConfig.Id.ToUpperInvariant()}.DESC", DNAStatueConfig.Description);
				Strings.Add($"STRINGS.BUILDINGS.PREFABS.{DNAStatueConfig.Id.ToUpperInvariant()}.EFFECT", DNAStatueConfig.Effect);

				ModUtil.AddBuildingToPlanScreen("Furniture", DNAStatueConfig.Id);
			}
		}

		[HarmonyPatch(typeof(Db))]
		[HarmonyPatch("Initialize")]
		public static class Db_Initialize_Patch
		{
			public static void Prefix()
			{
				var luxuryTech = new List<string>(Database.Techs.TECH_GROUPING["Luxury"]) { DNAStatueConfig.Id };
				Database.Techs.TECH_GROUPING["Luxury"] = luxuryTech.ToArray();
			}
		}
	}
}
