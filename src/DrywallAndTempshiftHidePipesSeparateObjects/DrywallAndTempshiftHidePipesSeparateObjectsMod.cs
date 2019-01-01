using Harmony;
using STRINGS;
using System;
using System.Collections.Generic;
using System.Linq;

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

				ModUtil.AddBuildingToPlanScreen("Utilities", DrywallHidePipesConfig.ID);
				ModUtil.AddBuildingToPlanScreen("Utilities", TempshiftHidesPipesConfig.ID);
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

		[HarmonyPatch(typeof(KSerialization.Manager), "GetType", new Type[] { typeof(string) })]
		public static class DrywallAndTempshiftHidePipesSeparateObjectsSerializationPatch
		{
			public static void Postfix(string type_name, ref Type __result)
			{
				if (type_name == "DrywallHidesPipes.ZoneTileClone" || type_name == "DrywallAndTempshiftHidePipesSeparateObjects.ZoneTileClone")
				{
					__result = typeof(ZoneTileClone);
				}
			}
		}

		[HarmonyPatch(typeof(CrownMouldingConfig), "CreateBuildingDef")]
		public static class CrownMouldingConfigPatch
		{
			public static void Postfix(ref BuildingDef __result)
			{
				__result.ObjectLayer = ObjectLayer.Building;
				__result.SceneLayer = Grid.SceneLayer.BuildingBack;
			}
		}
	}
}