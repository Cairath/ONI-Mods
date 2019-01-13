using System.Collections.Generic;
using Harmony;
using TUNING;

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

				AddBuildingToPlanScreen("Base", SteelLadderConfig.Id);
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

		private static void AddBuildingToPlanScreen(HashedString category, string buildingId)
		{
			var index = BUILDINGS.PLANORDER.FindIndex(x => x.category == category);

			if (index == -1)
				return;

			(BUILDINGS.PLANORDER[index].data as IList<string>)?.Add(buildingId);
		}
	}
}