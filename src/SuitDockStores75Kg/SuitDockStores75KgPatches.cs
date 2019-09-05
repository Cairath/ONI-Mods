using Harmony;
using UnityEngine;
using static CaiLib.Logger.Logger;

namespace SuitDockStores75Kg
{
	public static class SuitDockStores75KgPatches
	{
		public static class Mod_OnLoad
		{
			public static void OnLoad()
			{
				LogInit();
			}
		}

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
