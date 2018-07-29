using System;
using System.Collections.Generic;
using Harmony;

namespace Clock
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

				List<string> buldings = new List<string>((string[])TUNING.BUILDINGS.PLANORDER[8].data) { ClockConfig.ID };
				TUNING.BUILDINGS.PLANORDER[8].data = buldings.ToArray();

				TUNING.BUILDINGS.COMPONENT_DESCRIPTION_ORDER.Add(ClockConfig.ID);
			}

			private static void Postfix()
			{
				object obj = Activator.CreateInstance(typeof(ClockConfig));
				BuildingConfigManager.Instance.RegisterBuilding(obj as IBuildingConfig);
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
