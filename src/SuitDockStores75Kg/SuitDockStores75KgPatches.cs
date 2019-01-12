using Harmony;
using UnityEngine;

namespace SuitDockStores75Kg
{
	public class SuitDockStores75KgPatches
	{
		[HarmonyPatch(typeof(SuitLockerConfig))]
		[HarmonyPatch("ConfigureBuildingTemplate")]
		public class SuitLockerConfig_ConfigureBuildingTemplate_Patch
		{
			private static void Postfix(ref GameObject go)
			{
				var conduitConsumer = go.AddOrGet<ConduitConsumer>();
				conduitConsumer.capacityKG = 75f;
			}
		}

		[HarmonyPatch(typeof(JetSuitLockerConfig))]
		[HarmonyPatch("ConfigureBuildingTemplate")]
		public class JetSuitLockerConfig_ConfigureBuildingTemplate_Patch
		{
			private static void Postfix(ref GameObject go)
			{
				var conduitConsumer = go.AddOrGet<ConduitConsumer>();
				conduitConsumer.capacityKG = 75f;
			}
		}
	}
}
