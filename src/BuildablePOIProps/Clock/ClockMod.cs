using System;
using System.Collections.Generic;
using System.Linq;
using Harmony;

namespace BuildablePOIProps.Clock
{
	public class ClockMod
	{
		[HarmonyPatch(typeof(GeneratedBuildings), "LoadGeneratedBuildings")]
		public class ClockBuildingsPatch
		{
			private static void Prefix()
			{
				Strings.Add("STRINGS.BUILDINGS.PREFABS.CLOCK.NAME", "Clock");
				Strings.Add("STRINGS.BUILDINGS.PREFABS.CLOCK.DESC", "A simple wall clock.");
				Strings.Add("STRINGS.BUILDINGS.PREFABS.CLOCK.EFFECT", "A pretty clock for your wall.");

				ModUtil.AddBuildingToPlanScreen("Furniture", ClockConfig.ID);
			}
		}

		[HarmonyPatch(typeof(Db), "Initialize")]
		public class ClockDbPatch
		{
			private static void Prefix()
			{
				List<string> ls = new List<string>(Database.Techs.TECH_GROUPING["Luxury"]) { ClockConfig.ID };
				Database.Techs.TECH_GROUPING["Luxury"] = ls.ToArray();
			}
		}
	}
}
