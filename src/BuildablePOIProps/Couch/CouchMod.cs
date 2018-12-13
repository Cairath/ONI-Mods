using System;
using System.Collections.Generic;
using System.Linq;
using Harmony;

namespace BuildablePOIProps.Couch
{
	public class CouchMod
	{
		[HarmonyPatch(typeof(GeneratedBuildings), "LoadGeneratedBuildings")]
		public class CouchBuildingsPatch
		{
			private static void Prefix()
			{
				Strings.Add("STRINGS.BUILDINGS.PREFABS.COUCH.NAME", "Couch");
				Strings.Add("STRINGS.BUILDINGS.PREFABS.COUCH.DESC", "Comfier than it looks!");
				Strings.Add("STRINGS.BUILDINGS.PREFABS.COUCH.EFFECT", "Perfect for some relaxation.");

				ModUtil.AddBuildingToPlanScreen("Furniture", CouchConfig.ID);
			}
		}

		[HarmonyPatch(typeof(Db), "Initialize")]
		public class CouchDbPatch
		{
			private static void Prefix()
			{
				List<string> ls = new List<string>(Database.Techs.TECH_GROUPING["Luxury"]) { CouchConfig.ID };
				Database.Techs.TECH_GROUPING["Luxury"] = ls.ToArray();
			}
		}
	}
}