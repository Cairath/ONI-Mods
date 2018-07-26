using System;
using System.Collections.Generic;
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

				List<string> conveyorBuildings =
					new List<string>((string[])TUNING.BUILDINGS.PLANORDER[12].data) { SolidConduitFilterConfig.ID };
				TUNING.BUILDINGS.PLANORDER[12].data = conveyorBuildings.ToArray();
				TUNING.BUILDINGS.COMPONENT_DESCRIPTION_ORDER.Add(SolidConduitFilterConfig.ID);
			}

			private static void Postfix()
			{
				object obj = Activator.CreateInstance(typeof(SolidConduitFilterConfig));
				BuildingConfigManager.Instance.RegisterBuilding(obj as IBuildingConfig);
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
