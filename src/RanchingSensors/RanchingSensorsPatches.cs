using System;
using System.Collections.Generic;
using Harmony;

namespace RanchingSensors
{
	public static class RanchingSensorsPatches
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
				Strings.Add($"STRINGS.BUILDINGS.PREFABS.{CrittersSensorConfig.Id.ToUpperInvariant()}.NAME", CrittersSensorConfig.DisplayName);
				Strings.Add($"STRINGS.BUILDINGS.PREFABS.{CrittersSensorConfig.Id.ToUpperInvariant()}.DESC", CrittersSensorConfig.Description);
				Strings.Add($"STRINGS.BUILDINGS.PREFABS.{CrittersSensorConfig.Id.ToUpperInvariant()}.EFFECT", CrittersSensorConfig.Effect);

				Strings.Add($"STRINGS.BUILDINGS.PREFABS.{EggsSensorConfig.Id.ToUpperInvariant()}.NAME", EggsSensorConfig.DisplayName);
				Strings.Add($"STRINGS.BUILDINGS.PREFABS.{EggsSensorConfig.Id.ToUpperInvariant()}.DESC", EggsSensorConfig.Description);
				Strings.Add($"STRINGS.BUILDINGS.PREFABS.{EggsSensorConfig.Id.ToUpperInvariant()}.EFFECT", EggsSensorConfig.Effect);

				ModUtil.AddBuildingToPlanScreen("Automation", CrittersSensorConfig.Id);
				ModUtil.AddBuildingToPlanScreen("Automation", EggsSensorConfig.Id);
			}

			[HarmonyPatch(typeof(Db))]
			[HarmonyPatch("Initialize")]
			public static class Db_Initialize_Patch
			{
				public static void Prefix()
				{
					var animalTech = new List<string>(Database.Techs.TECH_GROUPING["AnimalControl"]) { CrittersSensorConfig.Id, EggsSensorConfig.Id };
					Database.Techs.TECH_GROUPING["AnimalControl"] = animalTech.ToArray();
				}
			}

			[HarmonyPatch(typeof(KSerialization.Manager))]
			[HarmonyPatch("GetType")]
			[HarmonyPatch(new[] { typeof(string) })]
			public static class KSerializationManager_GetType_Patch
			{
				public static void Postfix(string type_name, ref Type __result)
				{
					if (type_name == "RanchingSensors.CrittersSensor")
					{
						__result = typeof(CrittersSensor);
					}

					if (type_name == "RanchingSensors.EggsSensor")
					{
						__result = typeof(EggsSensor);
					}
				}
			}
		}
	}
}
