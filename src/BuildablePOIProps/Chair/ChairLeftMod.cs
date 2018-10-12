using System;
using System.Collections.Generic;
using System.Linq;
using BuildablePOIProps.DNAStatue;
using Harmony;

namespace BuildablePOIProps.Chair
{
	public class ChairMod
	{
		[HarmonyPatch(typeof(GeneratedBuildings), "LoadGeneratedBuildings")]
		public class ChairBuildingsPatch
		{
			private static void Prefix()
			{
				Strings.Add("STRINGS.BUILDINGS.PREFABS.CHAIRLEFT.NAME", "Chair (left)");
				Strings.Add("STRINGS.BUILDINGS.PREFABS.CHAIRLEFT.DESC", "A comfy chair.");
				Strings.Add("STRINGS.BUILDINGS.PREFABS.CHAIRLEFT.EFFECT", "So comfy!");

				Strings.Add("STRINGS.BUILDINGS.PREFABS.CHAIRRIGHT.NAME", "Chair (right)");
				Strings.Add("STRINGS.BUILDINGS.PREFABS.CHAIRRIGHT.DESC", "A comfy chair.");
				Strings.Add("STRINGS.BUILDINGS.PREFABS.CHAIRRIGHT.EFFECT", "So comfy!");

				List<string> category = (List<string>)TUNING.BUILDINGS.PLANORDER.First(po => po.category == PlanScreen.PlanCategory.Furniture).data;
				category.Add(ChairLeftConfig.ID);
				category.Add(ChairRightConfig.ID);
			}
		}

		[HarmonyPatch(typeof(Db), "Initialize")]
		public class ChairDbPatch
		{
			private static void Prefix()
			{
				List<string> ls = new List<string>(Database.Techs.TECH_GROUPING["Luxury"]) { ChairLeftConfig.ID, ChairRightConfig.ID };
				Database.Techs.TECH_GROUPING["Luxury"] = ls.ToArray();
			}
		}
	}
}