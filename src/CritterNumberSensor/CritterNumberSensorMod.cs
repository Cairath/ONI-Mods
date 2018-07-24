using System;
using System.Collections.Generic;
using Harmony;
using STRINGS;

namespace CritterNumberSensor
{
    public class CritterNumberSensorMod
    {
	    [HarmonyPatch(typeof(GeneratedBuildings), "LoadGeneratedBuildings")]
	    public class CritterNumberSensorBuildingsPatch
		{
		    private static void Prefix()
		    {
			    Strings.Add("STRINGS.BUILDINGS.PREFABS.LOGICCRITTERNUMBERSENSOR.NAME", "Critter Sensor");
			    Strings.Add("STRINGS.BUILDINGS.PREFABS.LOGICCRITTERNUMBERSENSOR.DESC", "Counts up the number of critters and eggs in the room.");
			    Strings.Add("STRINGS.BUILDINGS.PREFABS.LOGICCRITTERNUMBERSENSOR.EFFECT", "Becomes " + UI.FormatAsLink("Active", "LOGIC") + " or on " + UI.FormatAsLink("Standby", "LOGIC") + " when the number of critters and eggs in the room enters the chosen range.");

			    List<string> logicBuildings = new List<string>((string[])TUNING.BUILDINGS.PLANORDER[11].data) { CritterNumberSensorConfig.ID };
			    TUNING.BUILDINGS.PLANORDER[11].data = logicBuildings.ToArray();
			    TUNING.BUILDINGS.COMPONENT_DESCRIPTION_ORDER.Add(CritterNumberSensorConfig.ID);
		    }

		    private static void Postfix()
		    {
			    object obj = Activator.CreateInstance(typeof(CritterNumberSensorConfig));
			    BuildingConfigManager.Instance.RegisterBuilding(obj as IBuildingConfig);
		    }	

			[HarmonyPatch(typeof(Db), "Initialize")]
			public class CritterNumberSensorDbPatch
			{
				private static void Prefix()
				{
					List<string> ls = new List<string>(Database.Techs.TECH_GROUPING["AnimalControl"]) { CritterNumberSensorConfig.ID };
					Database.Techs.TECH_GROUPING["AnimalControl"] = ls.ToArray();
				}
			}

			[HarmonyPatch(typeof(KSerialization.Manager), "GetType", new Type[] { typeof(string) })]
			public static class CritterNumberSensorSerializationPatch
			{
				[HarmonyPostfix]
				public static void GetType(string type_name, ref Type __result)
				{
					if (type_name == "CritterNumberSensor.CritterNumberSensor")
					{
						__result = typeof(CritterNumberSensor);
					}
				}
			}
		}
	}
}
