using System.Collections.Generic;
using Harmony;

namespace BuildablePOIProps.DNAStatue
{
	public class DNAStatueMod
	{
		[HarmonyPatch(typeof(GeneratedBuildings), "LoadGeneratedBuildings")]
		public class DNAStatueBuildingsPatch
		{
			private static void Prefix()
			{
				Strings.Add("STRINGS.BUILDINGS.PREFABS.DNASTATUE.NAME", "DNA Statue");
				Strings.Add("STRINGS.BUILDINGS.PREFABS.DNASTATUE.DESC", "An enormous statue of a DNA chain.");
				Strings.Add("STRINGS.BUILDINGS.PREFABS.DNASTATUE.EFFECT", "Big and difficult to build.");

				ModUtil.AddBuildingToPlanScreen("Furniture", DNAStatueConfig.ID);
			}
		}

		[HarmonyPatch(typeof(Db), "Initialize")]
		public class DNAStatueDbPatch
		{
			private static void Prefix()
			{
				List<string> ls = new List<string>(Database.Techs.TECH_GROUPING["Luxury"]) { DNAStatueConfig.ID };
				Database.Techs.TECH_GROUPING["Luxury"] = ls.ToArray();
			}
		}
	}
}