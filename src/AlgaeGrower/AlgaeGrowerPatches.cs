using System;
using System.Collections.Generic;
using Database;
using Harmony;
using static CaiLib.Logger.Logger;

namespace AlgaeGrower
{
	public class AlgaeGrowerPatches
	{
		public static class Mod_OnLoad
		{
			public static void OnLoad()
			{
				LogInit(ModInfo.Name, ModInfo.Version);
			}
		}

		[HarmonyPatch(typeof(GeneratedBuildings))]
		[HarmonyPatch("LoadGeneratedBuildings")]
		public static class GeneratedBuildings_LoadGeneratedBuildings_Patch
		{
			public static void Prefix()
			{
				Strings.Add($"STRINGS.BUILDINGS.PREFABS.{AlgaeGrowerConfig.Id.ToUpperInvariant()}.NAME", AlgaeGrowerConfig.DisplayName);
				Strings.Add($"STRINGS.BUILDINGS.PREFABS.{AlgaeGrowerConfig.Id.ToUpperInvariant()}.DESC", AlgaeGrowerConfig.Description);
				Strings.Add($"STRINGS.BUILDINGS.PREFABS.{AlgaeGrowerConfig.Id.ToUpperInvariant()}.EFFECT", AlgaeGrowerConfig.Effect);

				ModUtil.AddBuildingToPlanScreen("Oxygen", AlgaeGrowerConfig.Id);
			}
		}

		[HarmonyPatch(typeof(Db))]
		[HarmonyPatch("Initialize")]
		public static class Db_Initialize_Patch
		{
			public static void Prefix()
			{
				var tech = new List<string>(Techs.TECH_GROUPING["FarmingTech"]) { AlgaeGrowerConfig.Id };
				Techs.TECH_GROUPING["FarmingTech"] = tech.ToArray();
			}
		}

		[HarmonyPatch(typeof(KSerialization.Manager))]
		[HarmonyPatch("GetType")]
		[HarmonyPatch(new[] { typeof(string) })]
		public static class KSerializationManager_GetType_Patch
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
