using System;
using System.Collections.Generic;
using System.Linq;
using Harmony;

namespace BuildablePOIProps.ComputerDesk
{
	public class ComputerDeskMod
	{
		[HarmonyPatch(typeof(GeneratedBuildings), "LoadGeneratedBuildings")]
		public class ComputerDeskBuildingsPatch
		{
			private static void Prefix()
			{
				Strings.Add("STRINGS.BUILDINGS.PREFABS.COMPUTERDESK.NAME", "Computer desk");
				Strings.Add("STRINGS.BUILDINGS.PREFABS.COMPUTERDESK.DESC", "An intact office desk, decorated with several personal belongings and a barely functioning computer.");
				Strings.Add("STRINGS.BUILDINGS.PREFABS.COMPUTERDESK.EFFECT", "Does it work? Who knows.");
				
				List<string> category = (List<string>) TUNING.BUILDINGS.PLANORDER.First(po => po.category == PlanScreen.PlanCategory.Furniture).data;
				category.Add(ComputerDeskConfig.ID);
			}
		}

		[HarmonyPatch(typeof(Db), "Initialize")]
		public class ComputerDeskDbPatch
		{
			private static void Prefix()
			{
				List<string> ls = new List<string>(Database.Techs.TECH_GROUPING["Luxury"]) { ComputerDeskConfig.ID };
				Database.Techs.TECH_GROUPING["Luxury"] = ls.ToArray();
			}
		}
	}
}