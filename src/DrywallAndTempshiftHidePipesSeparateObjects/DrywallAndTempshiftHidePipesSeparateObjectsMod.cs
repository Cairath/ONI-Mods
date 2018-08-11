using Harmony;
using STRINGS;
using System;
using System.Collections.Generic;

namespace DrywallAndTempshiftHidePipesSeparateObjects
{
	public class DrywallAndTempshiftHidePipesSeparateObjectsMod
	{
		[HarmonyPatch(typeof(GeneratedBuildings), "LoadGeneratedBuildings")]
		public class DrywallAndTempshiftHidePipesSeparateObjectsBuildingsPatch
		{
			private static void Prefix()
			{
				Strings.Add("STRINGS.BUILDINGS.PREFABS.EXTERIORWALLHIDESPIPES.NAME", BUILDINGS.PREFABS.EXTERIORWALL.NAME + " (hides pipes)");
				Strings.Add("STRINGS.BUILDINGS.PREFABS.EXTERIORWALLHIDESPIPES.DESC", BUILDINGS.PREFABS.EXTERIORWALL.DESC);
				Strings.Add("STRINGS.BUILDINGS.PREFABS.EXTERIORWALLHIDESPIPES.EFFECT", BUILDINGS.PREFABS.EXTERIORWALL.EFFECT);

				Strings.Add("STRINGS.BUILDINGS.PREFABS.THERMALBLOCKHIDESPIPES.NAME", BUILDINGS.PREFABS.THERMALBLOCK.NAME + " (hides pipes)");
				Strings.Add("STRINGS.BUILDINGS.PREFABS.THERMALBLOCKHIDESPIPES.DESC", BUILDINGS.PREFABS.THERMALBLOCK.DESC);
				Strings.Add("STRINGS.BUILDINGS.PREFABS.THERMALBLOCKHIDESPIPES.EFFECT", BUILDINGS.PREFABS.THERMALBLOCK.EFFECT);

				List<string> ls = new List<string>((string[])TUNING.BUILDINGS.PLANORDER[10].data) { DrywallHidePipesConfig.ID, TempshiftHidesPipesConfig.ID };
				TUNING.BUILDINGS.PLANORDER[10].data = ls.ToArray();

				TUNING.BUILDINGS.COMPONENT_DESCRIPTION_ORDER.Add(DrywallHidePipesConfig.ID);
				TUNING.BUILDINGS.COMPONENT_DESCRIPTION_ORDER.Add(TempshiftHidesPipesConfig.ID);
			}

			private static void Postfix()
			{
				object obj = Activator.CreateInstance(typeof(DrywallHidePipesConfig));
				BuildingConfigManager.Instance.RegisterBuilding(obj as IBuildingConfig);

				object obj2 = Activator.CreateInstance(typeof(TempshiftHidesPipesConfig));
				BuildingConfigManager.Instance.RegisterBuilding(obj2 as IBuildingConfig);
			}
		}

		[HarmonyPatch(typeof(Db), "Initialize")]
		public class DrywallAndTempshiftHidePipesSeparateObjectsDbPatch
		{
			private static void Prefix()
			{
				List<string> l = new List<string>(Database.Techs.TECH_GROUPING["Luxury"]) { DrywallHidePipesConfig.ID };
				Database.Techs.TECH_GROUPING["Luxury"] = l.ToArray();

				List<string> ro = new List<string>(Database.Techs.TECH_GROUPING["RefinedObjects"]) { TempshiftHidesPipesConfig.ID };
				Database.Techs.TECH_GROUPING["RefinedObjects"] = ro.ToArray();

			}
		}

		public static class DrywallAndTempshiftHidePipesSeparateObjectsSerializationPatch
		{
			[HarmonyPostfix]
			public static void GetType(string type_name, ref Type __result)
			{
				if (type_name == "DrywallHidesPipes.ZoneTileClone" || type_name == "DrywallAndTempshiftHidePipesSeparateObjects.ZoneTileClone")
				{
					__result = typeof(ZoneTileClone);
				}
			}
		}
	}
}