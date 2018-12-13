using System;
using System.Collections.Generic;
using System.Linq;
using Harmony;

namespace BuildablePOIProps.TableWithChairs
{
	public class TableWithChairsMod
	{
		[HarmonyPatch(typeof(GeneratedBuildings), "LoadGeneratedBuildings")]
		public class TableWithChairsBuildingsPatch
		{
			private static void Prefix()
			{
				Strings.Add("STRINGS.BUILDINGS.PREFABS.TABLEWITHCHAIRS.NAME", "Table");
				Strings.Add("STRINGS.BUILDINGS.PREFABS.TABLEWITHCHAIRS.DESC", "A table and some chairs.");
				Strings.Add("STRINGS.BUILDINGS.PREFABS.TABLEWITHCHAIRS.EFFECT", "Perfect for a break.");

				ModUtil.AddBuildingToPlanScreen("Furniture", TableWithChairsConfig.ID);
			}
		}

		[HarmonyPatch(typeof(Db), "Initialize")]
		public class TableWithChairsDbPatch
		{
			private static void Prefix()
			{
				List<string> ls = new List<string>(Database.Techs.TECH_GROUPING["Luxury"]) { TableWithChairsConfig.ID };
				Database.Techs.TECH_GROUPING["Luxury"] = ls.ToArray();
			}
		}
	}
}