using System;
using System.Collections.Generic;
using Harmony;

namespace PipedAlgaeTerrarium
{
	public class PipedAlgaeTerrariumPatches
	{
		[HarmonyPatch(typeof(SplashMessageScreen))]
		[HarmonyPatch("OnPrefabInit")]
		public static class SplashMessageScreen_OnPrefabInit_Patch
		{
			public static void Postfix()
			{
				CaiLib.ModCounter.ModCounter.Hit(ModInfo.Name, ModInfo.Version);
				CaiLib.Logger.LogInit(ModInfo.Name, ModInfo.Version);
			}
		}

		[HarmonyPatch(typeof(GeneratedBuildings))]
		[HarmonyPatch("LoadGeneratedBuildings")]
		public static class GeneratedBuildings_LoadGeneratedBuildings_Patch
		{
			public static void Prefix()
			{
				Strings.Add($"STRINGS.BUILDINGS.PREFABS.{PipedAlgaeTerrariumConfig.Id.ToUpperInvariant()}.NAME", PipedAlgaeTerrariumConfig.DisplayName);
				Strings.Add($"STRINGS.BUILDINGS.PREFABS.{PipedAlgaeTerrariumConfig.Id.ToUpperInvariant()}.DESC", PipedAlgaeTerrariumConfig.Description);
				Strings.Add($"STRINGS.BUILDINGS.PREFABS.{PipedAlgaeTerrariumConfig.Id.ToUpperInvariant()}.EFFECT", PipedAlgaeTerrariumConfig.Effect);

				ModUtil.AddBuildingToPlanScreen("Oxygen", PipedAlgaeTerrariumConfig.Id);
			}
		}

		[HarmonyPatch(typeof(Db))]
		[HarmonyPatch("Initialize")]
		public static class Db_Initialize_Patch
		{
			public static void Prefix()
			{
				var tech = new List<string>(Database.Techs.TECH_GROUPING["FarmingTech"]) { PipedAlgaeTerrariumConfig.Id };
				Database.Techs.TECH_GROUPING["FarmingTech"] = tech.ToArray();
			}
		}

		[HarmonyPatch(typeof(KSerialization.Manager))]
		[HarmonyPatch("GetType")]
		[HarmonyPatch(new[] { typeof(string) })]
		public static class KSerializationManager_GetType_Patch
		{
			public static void Postfix(string type_name, ref Type __result)
			{
				if (type_name == "PipedAlgaeTerrarium.PipedAlgaeTerrarium")
				{
					__result = typeof(PipedAlgaeTerrarium);
				}
			}
		}
	}
}
