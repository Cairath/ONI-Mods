using System;
using System.Collections.Generic;
using Harmony;

namespace FlowSplitters
{
	public class FlowSplittersMod
	{
		[HarmonyPatch(typeof(GeneratedBuildings), "LoadGeneratedBuildings")]
		public class FlowSplittersBuildingsPatch
		{
			private static void Prefix()
			{
				Strings.Add("STRINGS.BUILDINGS.PREFABS.LIQUIDSPLITTER.NAME", "Liquid Splitter");
				Strings.Add("STRINGS.BUILDINGS.PREFABS.LIQUIDSPLITTER.DESC", "Splits liquids equally in two pipes. If one the output pipes can't handle half to the input, the emtpier pipe will receive it.");
				Strings.Add("STRINGS.BUILDINGS.PREFABS.LIQUIDSPLITTER.EFFECT", "Have you ever wanted to have your liquids in two places at once?");

				Strings.Add("STRINGS.BUILDINGS.PREFABS.GASSPLITTER.NAME", "Gas Splitter");
				Strings.Add("STRINGS.BUILDINGS.PREFABS.GASSPLITTER.DESC", "Splits gases equally in two pipes. If one the output pipes can't handle half to the input, the emtpier pipe will receive it.");
				Strings.Add("STRINGS.BUILDINGS.PREFABS.GASSPLITTER.EFFECT", "Have you ever wanted to have your gases in two places at once?");

				List<string> liquid =
					new List<string>((string[])TUNING.BUILDINGS.PLANORDER[4].data) { LiquidSplitterConfig.ID};
				TUNING.BUILDINGS.PLANORDER[4].data = liquid.ToArray();

				List<string> gas =
					new List<string>((string[])TUNING.BUILDINGS.PLANORDER[5].data) {  GasSplitterConfig.ID };
				TUNING.BUILDINGS.PLANORDER[5].data = gas.ToArray();


				TUNING.BUILDINGS.COMPONENT_DESCRIPTION_ORDER.Add(LiquidSplitterConfig.ID);
				TUNING.BUILDINGS.COMPONENT_DESCRIPTION_ORDER.Add(GasSplitterConfig.ID);
			}

			private static void Postfix()
			{
				object obj = Activator.CreateInstance(typeof(LiquidSplitterConfig));
				BuildingConfigManager.Instance.RegisterBuilding(obj as IBuildingConfig);

				object obj2 = Activator.CreateInstance(typeof(GasSplitterConfig));
				BuildingConfigManager.Instance.RegisterBuilding(obj2 as IBuildingConfig);
			}
		}

		[HarmonyPatch(typeof(Db), "Initialize")]
		public class FlowSplittersDbPatch
		{
			private static void Prefix()
			{
				List<string> liquid = new List<string>(Database.Techs.TECH_GROUPING["LiquidPiping"]) { LiquidSplitterConfig.ID };
				Database.Techs.TECH_GROUPING["LiquidPiping"] = liquid.ToArray();

				List<string> gas = new List<string>(Database.Techs.TECH_GROUPING["GasPiping"]) { GasSplitterConfig.ID };
				Database.Techs.TECH_GROUPING["GasPiping"] = gas.ToArray();

			}
		}

		[HarmonyPatch(typeof(KSerialization.Manager), "GetType", new Type[] { typeof(string) })]
		public static class FlowSplittersSerializationPatch
		{
			[HarmonyPostfix]
			public static void GetType(string type_name, ref Type __result)
			{
				if (type_name == "FlowSplitters.FlowSplitter")
				{
					__result = typeof(FlowSplitter);
				}
			}
		}
	}
}
