using System;
using System.Collections.Generic;
using System.Linq;
using BuildablePOIProps.TableWithChairs;
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

				List<string> category = (List<string>)TUNING.BUILDINGS.PLANORDER.First(po => po.category == PlanScreen.PlanCategory.Furniture).data;
				category.Add(DNAStatueConfig.ID);
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