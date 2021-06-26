using HarmonyLib;
using UnityEngine;

namespace SuitDockStores75Kg
{
	public static class SuitDockStores75KgPatches
	{
		[HarmonyPatch(typeof(SuitLockerConfig))]
		[HarmonyPatch(nameof(SuitLockerConfig.ConfigureBuildingTemplate))]
		public static class SuitLockerConfig_ConfigureBuildingTemplate_Patch
		{
			public static void Postfix(ref GameObject go)
			{
				var conduitConsumer = go.AddOrGet<ConduitConsumer>();
				conduitConsumer.capacityKG = 75f;
			}
		}

		[HarmonyPatch(typeof(JetSuitLockerConfig))]
		[HarmonyPatch(nameof(JetSuitLockerConfig.ConfigureBuildingTemplate))]
		public static class JetSuitLockerConfig_ConfigureBuildingTemplate_Patch
		{
			public static void Postfix(ref GameObject go)
			{
				var conduitConsumer = go.AddOrGet<ConduitConsumer>();
				conduitConsumer.capacityKG = 75f;
			}
		}
	}
}
