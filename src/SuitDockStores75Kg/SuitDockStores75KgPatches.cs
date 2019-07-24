using Harmony;
using UnityEngine;

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
				CaiLib.Logger.Logger.LogInit(ModInfo.Name, ModInfo.Version);
			}
		}

		[HarmonyPatch(typeof(SuitLockerConfig))]
		[HarmonyPatch("ConfigureBuildingTemplate")]
		public static class SuitLockerConfig_ConfigureBuildingTemplate_Patch
		{
			public static void Postfix(ref GameObject go)
			{
				var conduitConsumer = go.AddOrGet<ConduitConsumer>();
				conduitConsumer.capacityKG = 75f;
			}
		}

		[HarmonyPatch(typeof(JetSuitLockerConfig))]
		[HarmonyPatch("ConfigureBuildingTemplate")]
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
