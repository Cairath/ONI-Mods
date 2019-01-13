using System;
using System.Collections.Generic;
using Harmony;

namespace FlowSplitters
{
	public class FlowSplittersPatches
	{
		[HarmonyPatch(typeof(GeneratedBuildings))]
		[HarmonyPatch("LoadGeneratedBuildings")]
		public class GeneratedBuildings_LoadGeneratedBuildings_Patch
		{
			private static void Prefix()
			{
				Strings.Add($"STRINGS.BUILDINGS.PREFABS.{LiquidSplitterConfig.Id.ToUpperInvariant()}.NAME", LiquidSplitterConfig.DisplayName);
				Strings.Add($"STRINGS.BUILDINGS.PREFABS.{LiquidSplitterConfig.Id.ToUpperInvariant()}.DESC", LiquidSplitterConfig.Description);
				Strings.Add($"STRINGS.BUILDINGS.PREFABS.{LiquidSplitterConfig.Id.ToUpperInvariant()}.EFFECT", LiquidSplitterConfig.Effect);

				Strings.Add($"STRINGS.BUILDINGS.PREFABS.{GasSplitterConfig.Id.ToUpperInvariant()}.NAME", GasSplitterConfig.DisplayName);
				Strings.Add($"STRINGS.BUILDINGS.PREFABS.{GasSplitterConfig.Id.ToUpperInvariant()}.DESC", GasSplitterConfig.Description);
				Strings.Add($"STRINGS.BUILDINGS.PREFABS.{GasSplitterConfig.Id.ToUpperInvariant()}.EFFECT", GasSplitterConfig.Effect);

				ModUtil.AddBuildingToPlanScreen("Plumbing", LiquidSplitterConfig.Id);
				ModUtil.AddBuildingToPlanScreen("HVAC", GasSplitterConfig.Id);
			}
		}

		[HarmonyPatch(typeof(Db))]
		[HarmonyPatch("Initialize")]
		public class Db_Initialize_Patch
		{
			private static void Prefix()
			{
				var liquid = new List<string>(Database.Techs.TECH_GROUPING["LiquidPiping"]) { LiquidSplitterConfig.Id };
				Database.Techs.TECH_GROUPING["LiquidPiping"] = liquid.ToArray();

				var gas = new List<string>(Database.Techs.TECH_GROUPING["GasPiping"]) { GasSplitterConfig.Id };
				Database.Techs.TECH_GROUPING["GasPiping"] = gas.ToArray();

			}
		}

		[HarmonyPatch(typeof(KSerialization.Manager))]
		[HarmonyPatch("GetType")]
		[HarmonyPatch(new[] { typeof(string) })]
		public static class FlowSplittersSerializationPatch
		{
			public static void Postfix(string type_name, ref Type __result)
			{
				if (type_name == "FlowSplitters.FlowSplitter")
				{
					__result = typeof(FlowSplitter);
				}
			}
		}
	}
}
