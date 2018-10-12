using System;
using System.Collections.Generic;
using System.Linq;
using Harmony;

namespace ConveyorFilter
{
	public class SolidConduitFilterMod
	{
		[HarmonyPatch(typeof(GeneratedBuildings), "LoadGeneratedBuildings")]
		public class SolidConduitFilterBuildingsPatch
		{
			private static void Prefix()
			{
				Strings.Add("STRINGS.BUILDINGS.PREFABS.SOLIDCONDUITFILTER.NAME", "Conveyor Rail Filter");
				Strings.Add("STRINGS.BUILDINGS.PREFABS.SOLIDCONDUITFILTER.DESC", "Filters incoming items on the Conveyor Rail. Filtered items (selected on the list) are put on the secondary output in the middle of the filter (icon not visible).");
				Strings.Add("STRINGS.BUILDINGS.PREFABS.SOLIDCONDUITFILTER.EFFECT", "Filters the Conveyor Rail by sending selected items to a separate output.");

				List<string> category = (List<string>)TUNING.BUILDINGS.PLANORDER.First(po => po.category == PlanScreen.PlanCategory.Conveyance).data;
				category.Add(SolidConduitFilterConfig.ID);
			}
		}

		[HarmonyPatch(typeof(Db), "Initialize")]
		public class SolidConduitFilterDbPatch
		{
			private static void Prefix()
			{
				List<string> ls = new List<string>(Database.Techs.TECH_GROUPING["SolidTransport"]) { SolidConduitFilterConfig.ID };
				Database.Techs.TECH_GROUPING["SolidTransport"] = ls.ToArray();
			}
		}

		[HarmonyPatch(typeof(KSerialization.Manager), "GetType", new Type[] { typeof(string) })]
		public static class SolidConduitFilterSerializationPatch
		{
			[HarmonyPostfix]
			public static void GetType(string type_name, ref Type __result)
			{
				if (type_name == "ConveyorFilter.SolidConduitFilter")
				{
					__result = typeof(SolidConduitFilter);
				}
			}
		}
	}
}
