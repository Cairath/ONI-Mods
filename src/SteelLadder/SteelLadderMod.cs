using System;
using System.Collections.Generic;
using System.Linq;
using Harmony;

namespace SteelLadder
{
	public class SteelLadderMod
	{
		[HarmonyPatch(typeof(GeneratedBuildings), "LoadGeneratedBuildings")]
		public class SteelLadderBuildingsPatch
		{
			private static void Prefix()
			{
				Strings.Add("STRINGS.BUILDINGS.PREFABS.STEELLADDER.NAME", "Steel Ladder");
				Strings.Add("STRINGS.BUILDINGS.PREFABS.STEELLADDER.DESC", "Durable steel ladder.");
				Strings.Add("STRINGS.BUILDINGS.PREFABS.STEELLADDER.EFFECT", "Increases Duplicant climbing speed.");
				
				ModUtil.AddBuildingToPlanScreen("Furniture", SteelLadderConfig.ID);
			}
		}

		[HarmonyPatch(typeof(Db), "Initialize")]
		public class SteelLadderDbPatch
		{
			private static void Prefix()
			{
				List<string> ls = new List<string>(Database.Techs.TECH_GROUPING["Luxury"]) { SteelLadderConfig.ID };
				Database.Techs.TECH_GROUPING["Luxury"] = ls.ToArray();
			}
		}
	}
}