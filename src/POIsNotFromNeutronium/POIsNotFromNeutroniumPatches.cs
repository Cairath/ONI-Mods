using Harmony;
using UnityEngine;

namespace POIsNotFromNeutronium
{
	public class POIsNotFromNeutroniumPatches
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

		[HarmonyPatch(typeof(GeneShufflerConfig), "CreatePrefab")]
		public static class GeneShufflerConfig_CreatePrefab_Patches
		{
			public static void Postfix(ref GameObject __result)
			{
				__result.GetComponent<PrimaryElement>().SetElement(SimHashes.Steel);
			}
		}

		[HarmonyPatch(typeof(PropClockConfig), "CreatePrefab")]
		public static class PropClockConfig_CreatePrefab_Patch
		{
			public static void Postfix(ref GameObject __result)
			{
				__result.GetComponent<PrimaryElement>().SetElement(SimHashes.Polypropylene);
			}
		}

		[HarmonyPatch(typeof(SetLockerConfig), "CreatePrefab")]
		public static class SetLockerConfig_CreatePrefab_Patch
		{
			public static void Postfix(ref GameObject __result)
			{
				__result.GetComponent<PrimaryElement>().SetElement(SimHashes.Steel);
			}
		}

		[HarmonyPatch(typeof(VendingMachineConfig), "CreatePrefab")]
		public static class VendingMachineConfig_CreatePrefab_Patch
		{
			public static void Postfix(ref GameObject __result)
			{
				__result.GetComponent<PrimaryElement>().SetElement(SimHashes.Steel);
			}
		}

		[HarmonyPatch(typeof(PropTableConfig), "CreatePrefab")]
		public static class PropTableConfig_CreatePrefab_Patch
		{
			public static void Postfix(ref GameObject __result)
			{
				__result.GetComponent<PrimaryElement>().SetElement(SimHashes.Polypropylene);
			}
		}

		[HarmonyPatch(typeof(LadderPOIConfig), "CreatePrefab")]
		public static class LadderPOIConfig_CreatePrefab_Patch
		{
			public static void Postfix(ref GameObject __result)
			{
				__result.GetComponent<PrimaryElement>().SetElement(SimHashes.Steel);
			}
		}
	}
}
