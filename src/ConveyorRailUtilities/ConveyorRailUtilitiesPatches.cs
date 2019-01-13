using System;
using System.Collections.Generic;
using ConveyorRailUtilities.Dropoff;
using ConveyorRailUtilities.Filter;
using ConveyorRailUtilities.Shutoff;
using Harmony;

namespace ConveyorRailUtilities
{
	public static class ConveyorRailUtilitiesPatches
	{
		[HarmonyPatch(typeof(GeneratedBuildings))]
		[HarmonyPatch("LoadGeneratedBuildings")]
		public static class GeneratedBuildings_LoadGeneratedBuildings_Patch
		{
			public static void Prefix()
			{
				Strings.Add($"STRINGS.BUILDINGS.PREFABS.{ ConveyorFilterConfig.Id.ToUpperInvariant()}.NAME", ConveyorFilterConfig.DisplayName);
				Strings.Add($"STRINGS.BUILDINGS.PREFABS.{ ConveyorFilterConfig.Id.ToUpperInvariant()}.DESC", ConveyorFilterConfig.Description);
				Strings.Add($"STRINGS.BUILDINGS.PREFABS.{ ConveyorFilterConfig.Id.ToUpperInvariant()}.EFFECT", ConveyorFilterConfig.Effect);

				Strings.Add($"STRINGS.BUILDINGS.PREFABS.{ ConveyorShutoffConfig.Id.ToUpperInvariant()}.NAME", ConveyorShutoffConfig.DisplayName);
				Strings.Add($"STRINGS.BUILDINGS.PREFABS.{ ConveyorShutoffConfig.Id.ToUpperInvariant()}.DESC", ConveyorShutoffConfig.Description);
				Strings.Add($"STRINGS.BUILDINGS.PREFABS.{ ConveyorShutoffConfig.Id.ToUpperInvariant()}.EFFECT", ConveyorShutoffConfig.Effect);

				Strings.Add($"STRINGS.BUILDINGS.PREFABS.{ConveyorDropoffConfig.Id.ToUpper()}.NAME", ConveyorDropoffConfig.DisplayName);
				Strings.Add($"STRINGS.BUILDINGS.PREFABS.{ConveyorDropoffConfig.Id.ToUpper()}.DESC", ConveyorDropoffConfig.Description);
				Strings.Add($"STRINGS.BUILDINGS.PREFABS.{ConveyorDropoffConfig.Id.ToUpper()}.EFFECT", ConveyorDropoffConfig.Effect);

				ModUtil.AddBuildingToPlanScreen("Conveyance", ConveyorFilterConfig.Id);
				ModUtil.AddBuildingToPlanScreen("Conveyance", ConveyorShutoffConfig.Id);
				ModUtil.AddBuildingToPlanScreen("Conveyance", ConveyorDropoffConfig.Id);
			}
		}

		[HarmonyPatch(typeof(Db))]
		[HarmonyPatch("Initialize")]
		public static class Db_Initialize_Patch
		{
			public static void Prefix()
			{
				var techs = new List<string>(Database.Techs.TECH_GROUPING["SolidTransport"]) { ConveyorFilterConfig.Id, ConveyorShutoffConfig.Id, ConveyorDropoffConfig.Id };
				Database.Techs.TECH_GROUPING["SolidTransport"] = techs.ToArray();
			}
		}

		[HarmonyPatch(typeof(KSerialization.Manager))]
		[HarmonyPatch("GetType")]
		[HarmonyPatch(new[] { typeof(string) })]
		public static class KSerializationManager_GetType_Patch
		{
			public static void Postfix(string type_name, ref Type __result)
			{
				if (type_name == "ConveyorRailUtilities.ConveyorFilter.ConveyorFilter")
				{
					__result = typeof(ConveyorFilter);
				}

				if (type_name == "ConveyorRailUtilities.ConveyorShutoff.ConveyorShutoff")
				{
					__result = typeof(ConveyorShutoff);
				}
			}
		}
	}
}
