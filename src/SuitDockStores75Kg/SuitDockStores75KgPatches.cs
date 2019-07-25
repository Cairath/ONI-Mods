using Harmony;
using UnityEngine;
using static CaiLib.Logger.Logger;

namespace SuitDockStores75Kg
{
	public static class SuitDockStores75KgPatches
	{
		[HarmonyPatch(typeof(SplashMessageScreen))]
		[HarmonyPatch("OnPrefabInit")]
		public static class SplashMessageScreen_OnPrefabInit_Patch
		{
			public static void Postfix()
			{
				LogInit(ModInfo.Name, ModInfo.Version);
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
