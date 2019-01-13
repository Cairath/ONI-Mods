using System.Collections.Generic;
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
				
				ModUtil.AddBuildingToPlanScreen("Furniture", ChairLeftConfig.ID);
			}
		}

		[HarmonyPatch(typeof(Db), "Initialize")]
		public class ChairDbPatch
		{
			private static void Prefix()
			{
				List<string> ls = new List<string>(Database.Techs.TECH_GROUPING["Luxury"]) { ChairLeftConfig.ID};
				Database.Techs.TECH_GROUPING["Luxury"] = ls.ToArray();
			}
		}
	}
}