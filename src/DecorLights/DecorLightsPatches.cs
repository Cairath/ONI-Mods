using Database;
using Harmony;
using System.Collections.Generic;

namespace DecorLights
{
	[HarmonyPatch(typeof(SplashMessageScreen))]
	[HarmonyPatch("OnPrefabInit")]
	public static class SplashMessageScreen_OnPrefabInit_Patch
	{
		public static void Postfix()
		{
			CaiLib.Logger.Logger.LogInit(ModInfo.Name, ModInfo.Version);
		}
	}

	public class DecorLightsPatches
	{
		[HarmonyPatch(typeof(GeneratedBuildings))]
		[HarmonyPatch("LoadGeneratedBuildings")]
		public static class GeneratedBuildings_LoadGeneratedBuildings_Patch
		{
			public static void Prefix()
			{
				Strings.Add($"STRINGS.BUILDINGS.PREFABS.{LavaLampConfig.Id.ToUpperInvariant()}.NAME", LavaLampConfig.DisplayName);
				Strings.Add($"STRINGS.BUILDINGS.PREFABS.{LavaLampConfig.Id.ToUpperInvariant()}.DESC", LavaLampConfig.Description);
				Strings.Add($"STRINGS.BUILDINGS.PREFABS.{LavaLampConfig.Id.ToUpperInvariant()}.EFFECT", LavaLampConfig.Effect);

				Strings.Add($"STRINGS.BUILDINGS.PREFABS.{SaltLampConfig.Id.ToUpperInvariant()}.NAME", SaltLampConfig.DisplayName);
				Strings.Add($"STRINGS.BUILDINGS.PREFABS.{SaltLampConfig.Id.ToUpperInvariant()}.DESC", SaltLampConfig.Description);
				Strings.Add($"STRINGS.BUILDINGS.PREFABS.{SaltLampConfig.Id.ToUpperInvariant()}.EFFECT", SaltLampConfig.Effect);

				Strings.Add($"STRINGS.BUILDINGS.PREFABS.{CeilingLampConfig.Id.ToUpperInvariant()}.NAME", CeilingLampConfig.DisplayName);
				Strings.Add($"STRINGS.BUILDINGS.PREFABS.{CeilingLampConfig.Id.ToUpperInvariant()}.DESC", CeilingLampConfig.Description);
				Strings.Add($"STRINGS.BUILDINGS.PREFABS.{CeilingLampConfig.Id.ToUpperInvariant()}.EFFECT", CeilingLampConfig.Effect);

				ModUtil.AddBuildingToPlanScreen("Furniture", LavaLampConfig.Id);
				ModUtil.AddBuildingToPlanScreen("Furniture", SaltLampConfig.Id);
				ModUtil.AddBuildingToPlanScreen("Furniture", CeilingLampConfig.Id);
			}
		}

		[HarmonyPatch(typeof(Db))]
		[HarmonyPatch("Initialize")]
		public static class Db_Initialize_Patch
		{
			public static void Prefix()
			{
				var tech = new List<string>(Techs.TECH_GROUPING["Luxury"]) { LavaLampConfig.Id, SaltLampConfig.Id, CeilingLampConfig.Id };
				Techs.TECH_GROUPING["Luxury"] = tech.ToArray();
			}
		}
	}
}
