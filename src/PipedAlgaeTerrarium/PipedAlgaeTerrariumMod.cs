using System;
using System.Collections.Generic;
using Harmony;
using STRINGS;

namespace PipedAlgaeTerrarium
{
	public class PipedAlgaeTerrariumMod
	{
		[HarmonyPatch(typeof(GeneratedBuildings), "LoadGeneratedBuildings")]
		public class PipedAlgaeTerrariumBuildingsPatch
		{
			private static void Prefix()
			{
				Strings.Add("STRINGS.BUILDINGS.PREFABS.ALGAEHABITATPIPED.NAME", "Piped Algae Terrarium");
				Strings.Add("STRINGS.BUILDINGS.PREFABS.ALGAEHABITATPIPED.DESC", "Algae colony, Duplicant colony... we're more alike than we are different.");
				Strings.Add("STRINGS.BUILDINGS.PREFABS.ALGAEHABITATPIPED.EFFECT", "Consumes " + (string) ELEMENTS.ALGAE.NAME + " to produce " + (string) ELEMENTS.OXYGEN.NAME + " and remove some " + (string) ELEMENTS.CARBONDIOXIDE.NAME + ".\n\nGains a 10 % efficiency boost in direct " + UI.FormatAsLink("Light", "LIGHT") + ".");

				List<string> oxygenBuildings = new List<string>((string[]) TUNING.BUILDINGS.PLANORDER[1].data) {PipedAlgaeTerrariumConfig.ID};
				TUNING.BUILDINGS.PLANORDER[1].data = oxygenBuildings.ToArray();
			}

			private static void Postfix()
			{
				object obj = Activator.CreateInstance(typeof(PipedAlgaeTerrariumConfig));
				BuildingConfigManager.Instance.RegisterBuilding(obj as IBuildingConfig);
			}
		}

		[HarmonyPatch(typeof(Db), "Initialize")]
		public class PipedAlgaeTerrariumDbPatch
		{
			private static void Prefix()
			{
				List<string> ls = new List<string>(Database.Techs.TECH_GROUPING["FarmingTech"]) { PipedAlgaeTerrariumConfig.ID };
				Database.Techs.TECH_GROUPING["FarmingTech"] = ls.ToArray();
			}
		}

		[HarmonyPatch(typeof(KSerialization.Manager), "GetType", new Type[] { typeof(string) })]
		public static class PipedAlgaeTerrariumSerializationPatch
		{
			[HarmonyPostfix]
			public static void GetType(string type_name, ref Type __result)
			{
				if (type_name == "PipedAlgaeTerrarium.PipedAlgaeTerrarium")
				{
					__result = typeof(PipedAlgaeTerrarium);
				}
			}
		}
	}
}