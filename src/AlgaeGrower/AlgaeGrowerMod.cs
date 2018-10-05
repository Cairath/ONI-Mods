using System;
using System.Collections.Generic;
using Database;
using Harmony;
using KSerialization;
using STRINGS;
using BUILDINGS = TUNING.BUILDINGS;

namespace AlgaeGrower
{
	public class AlgaeGrowerMod
	{
		[HarmonyPatch(typeof(GeneratedBuildings), "LoadGeneratedBuildings")]
		public class AlgaeGrowerBuildingsPatch
		{
			private static void Prefix()
			{
				Strings.Add("STRINGS.BUILDINGS.PREFABS.ALGAEGROWER.NAME", "Algae Grower");
				Strings.Add("STRINGS.BUILDINGS.PREFABS.ALGAEGROWER.DESC", "Algae colony, Duplicant colony... we're more alike than we are different.");
				Strings.Add("STRINGS.BUILDINGS.PREFABS.ALGAEGROWER.EFFECT", "Consumes " + ELEMENTS.FERTILIZER.NAME + ", " + ELEMENTS.CARBONDIOXIDE.NAME + " and " + ELEMENTS.WATER.NAME + " to grow " +  ELEMENTS.ALGAE.NAME + " and emit some " + ELEMENTS.OXYGEN.NAME + ".\n\nRequires " + UI.FormatAsLink("Light", "LIGHT") + " to grow.");

				List<string> oxygenBuildings = new List<string>((string[]) BUILDINGS.PLANORDER[1].data) {AlgaeGrowerConfig.ID};
				BUILDINGS.PLANORDER[1].data = oxygenBuildings.ToArray();
			}

			private static void Postfix()
			{
				object obj = Activator.CreateInstance(typeof(AlgaeGrowerConfig));
				BuildingConfigManager.Instance.RegisterBuilding(obj as IBuildingConfig);
			}
		}

		[HarmonyPatch(typeof(Db), "Initialize")]
		public class AlgaeGrowerDbPatch
		{
			private static void Prefix()
			{
				List<string> ls = new List<string>(Techs.TECH_GROUPING["FarmingTech"]) { AlgaeGrowerConfig.ID };
				Techs.TECH_GROUPING["FarmingTech"] = ls.ToArray();
			}
		}

		[HarmonyPatch(typeof(Manager), "GetType", new[] { typeof(string) })]
		public static class AlgaeGrowerSerializationPatch
		{
			public static void Postfix(string type_name, ref Type __result)
			{
				if (type_name == "AlgaeGrower.AlgaeGrower")
				{
					__result = typeof(AlgaeGrower);
				}
			}
		}
	}
}