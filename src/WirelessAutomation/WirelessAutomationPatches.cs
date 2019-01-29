using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Harmony;

namespace WirelessAutomation
{
    public class WirelessAutomationPatches
    {
	    [HarmonyPatch(typeof(Game), "OnPrefabInit")]
	    public static class GameOnPrefabInit
	    {
		    public static void Postfix(PauseScreen __instance)
		    {
			    WirelessAutomationManager.ResetEmittersList();
		    }
	    }

		[HarmonyPatch(typeof(GeneratedBuildings))]
		[HarmonyPatch("LoadGeneratedBuildings")]
		public class GeneratedBuildings_LoadGeneratedBuildings_Patch
		{
			public static void Prefix()
			{
				Strings.Add($"STRINGS.BUILDINGS.PREFABS.{WirelessAutomationEmitterConfig.Id.ToUpperInvariant()}.NAME", WirelessAutomationEmitterConfig.DisplayName);
				Strings.Add($"STRINGS.BUILDINGS.PREFABS.{WirelessAutomationEmitterConfig.Id.ToUpperInvariant()}.DESC", WirelessAutomationEmitterConfig.Description);
				Strings.Add($"STRINGS.BUILDINGS.PREFABS.{WirelessAutomationEmitterConfig.Id.ToUpperInvariant()}.EFFECT", WirelessAutomationEmitterConfig.Effect);

				Strings.Add($"STRINGS.BUILDINGS.PREFABS.{WirelessAutomationReceiverConfig.Id.ToUpperInvariant()}.NAME", WirelessAutomationReceiverConfig.DisplayName);
				Strings.Add($"STRINGS.BUILDINGS.PREFABS.{WirelessAutomationReceiverConfig.Id.ToUpperInvariant()}.DESC", WirelessAutomationReceiverConfig.Description);
				Strings.Add($"STRINGS.BUILDINGS.PREFABS.{WirelessAutomationReceiverConfig.Id.ToUpperInvariant()}.EFFECT", WirelessAutomationReceiverConfig.Effect);

				ModUtil.AddBuildingToPlanScreen("Automation", WirelessAutomationEmitterConfig.Id);
				ModUtil.AddBuildingToPlanScreen("Automation", WirelessAutomationReceiverConfig.Id);
			}

			[HarmonyPatch(typeof(Db))]
			[HarmonyPatch("Initialize")]
			public static class Db_Initialize_Patch
			{
				public static void Prefix()
				{
					var animalTech = new List<string>(Database.Techs.TECH_GROUPING["AnimalControl"]) { WirelessAutomationEmitterConfig.Id, WirelessAutomationReceiverConfig.Id };
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
					//if (type_name == "RanchingSensors.CrittersSensor")
					//{
					//	__result = typeof(CrittersSensor);
					//}

					//if (type_name == "RanchingSensors.EggsSensor")
					//{
					//	__result = typeof(EggsSensor);
					//}
				}
			}
		}
	}
}
