using System;
using System.Collections.Generic;
using System.Linq;
using Harmony;
using STRINGS;

namespace RanchingSensors
{
    public class RanchingSensorsMod
    {
	    [HarmonyPatch(typeof(GeneratedBuildings), "LoadGeneratedBuildings")]
	    public class RanchingSensorsBuildingsPatch
		{
		    private static void Prefix()
		    {
			    Strings.Add("STRINGS.BUILDINGS.PREFABS.CRITTERSSENSOR.NAME", "Live Critters Sensor");
			    Strings.Add("STRINGS.BUILDINGS.PREFABS.CRITTERSSENSOR.DESC", "Counts up the number of critters in the room.");
			    Strings.Add("STRINGS.BUILDINGS.PREFABS.CRITTERSSENSOR.EFFECT", "Becomes " + UI.FormatAsLink("Active", "LOGIC") + " or on " + UI.FormatAsLink("Standby", "LOGIC") + " depending on the number of live critters (no eggs) in a room.");

			    Strings.Add("STRINGS.BUILDINGS.PREFABS.EGGSSENSOR.NAME", "Egg Sensor");
			    Strings.Add("STRINGS.BUILDINGS.PREFABS.EGGSSENSOR.DESC", "Counts up the number of eggs in the room.");
			    Strings.Add("STRINGS.BUILDINGS.PREFABS.EGGSSENSOR.EFFECT", "Becomes " + UI.FormatAsLink("Active", "LOGIC") + " or on " + UI.FormatAsLink("Standby", "LOGIC") + " depending on the number of eggs in a room.");

			    ModUtil.AddBuildingToPlanScreen("Automation", CrittersSensorConfig.ID);
			    ModUtil.AddBuildingToPlanScreen("Automation", EggsSensorConfig.ID);
			}

			[HarmonyPatch(typeof(Db), "Initialize")]
			public class RanchingSensorsDbPatch
			{
				private static void Prefix()
				{
					List<string> ls = new List<string>(Database.Techs.TECH_GROUPING["AnimalControl"]) { CrittersSensorConfig.ID, EggsSensorConfig.ID };
					Database.Techs.TECH_GROUPING["AnimalControl"] = ls.ToArray();
				}
			}

			[HarmonyPatch(typeof(KSerialization.Manager), "GetType", new Type[] { typeof(string) })]
			public static class RanchingSensorsSerializationPatch
			{
				[HarmonyPostfix]
				public static void GetType(string type_name, ref Type __result)
				{
					if (type_name == "RanchingSensors.CrittersSensor")
					{
						__result = typeof(CrittersSensor);
					}

					if (type_name == "RanchingSensors.EggsSensor")
					{
						__result = typeof(EggsSensor);
					}
				}
			}
		}
	}
}
