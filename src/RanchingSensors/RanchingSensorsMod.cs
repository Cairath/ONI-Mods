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
			    Strings.Add("STRINGS.BUILDINGS.PREFABS.CRITTERSANDEGGSSENSOR.NAME", "Critters and Eggs Sensor");
			    Strings.Add("STRINGS.BUILDINGS.PREFABS.CRITTERSANDEGGSSENSOR.DESC", "Counts up the number of critters and eggs in the room.");
			    Strings.Add("STRINGS.BUILDINGS.PREFABS.CRITTERSANDEGGSSENSOR.EFFECT", "Becomes " + UI.FormatAsLink("Active", "LOGIC") + " or on " + UI.FormatAsLink("Standby", "LOGIC") + " when the number of critters and eggs in the room enters the chosen range.");

			    Strings.Add("STRINGS.BUILDINGS.PREFABS.CRITTERSSENSOR.NAME", "Critters Sensor");
			    Strings.Add("STRINGS.BUILDINGS.PREFABS.CRITTERSSENSOR.DESC", "Counts up the number of critters in the room.");
			    Strings.Add("STRINGS.BUILDINGS.PREFABS.CRITTERSSENSOR.EFFECT", "Becomes " + UI.FormatAsLink("Active", "LOGIC") + " or on " + UI.FormatAsLink("Standby", "LOGIC") + " when the number of critters in the room enters the chosen range.");

			    Strings.Add("STRINGS.BUILDINGS.PREFABS.EGGSSENSOR.NAME", "Eggs Sensor");
			    Strings.Add("STRINGS.BUILDINGS.PREFABS.EGGSSENSOR.DESC", "Counts up the number of eggs in the room.");
			    Strings.Add("STRINGS.BUILDINGS.PREFABS.EGGSSENSOR.EFFECT", "Becomes " + UI.FormatAsLink("Active", "LOGIC") + " or on " + UI.FormatAsLink("Standby", "LOGIC") + " when the number of eggs in the room enters the chosen range.");

			    List<string> category = (List<string>)TUNING.BUILDINGS.PLANORDER.First(po => po.category == PlanScreen.PlanCategory.Automation).data;
			    category.Add(CrittersAndEggsSensorConfig.ID);
			    category.Add(CrittersSensorConfig.ID);
			    category.Add(EggsSensorConfig.ID);
			}

			[HarmonyPatch(typeof(Db), "Initialize")]
			public class RanchingSensorsDbPatch
			{
				private static void Prefix()
				{
					List<string> ls = new List<string>(Database.Techs.TECH_GROUPING["AnimalControl"]) { CrittersAndEggsSensorConfig.ID, CrittersSensorConfig.ID, EggsSensorConfig.ID };
					Database.Techs.TECH_GROUPING["AnimalControl"] = ls.ToArray();
				}
			}

			[HarmonyPatch(typeof(KSerialization.Manager), "GetType", new Type[] { typeof(string) })]
			public static class RanchingSensorsSerializationPatch
			{
				[HarmonyPostfix]
				public static void GetType(string type_name, ref Type __result)
				{
					if (type_name == "RanchingSensors.CrittersAndEggsSensor")
					{
						__result = typeof(CrittersAndEggsSensor);
					}

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
