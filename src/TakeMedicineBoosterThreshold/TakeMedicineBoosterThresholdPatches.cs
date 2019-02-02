using Harmony;
using Klei.AI;
using UnityEngine;

namespace TakeMedicineBoosterThreshold
{
	public static class TakeMedicineBoosterThresholdPatches
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

		[HarmonyPatch(typeof(MedicinalPill))]
		[HarmonyPatch("CanBeTakenBy")]
		public static class MedicinalPill_CanBeTakenBy_Patch
		{
			public static void Postfix(MedicinalPill __instance, ref GameObject consumer, ref bool __result)
			{
				var component = consumer.GetComponent<Effects>();
				if (component == null || component.HasEffect(__instance.info.effect))
				{
					__result = false;
					return;
				}

				if (__instance.info.medicineType == MedicineInfo.MedicineType.Booster)
				{
					var amountInstance = Db.Get().Amounts.ImmuneLevel.Lookup(consumer);
					if (amountInstance != null)
					{
						__result = amountInstance.value < amountInstance.GetMax() * 0.8f;
						return;
					}

					__result = false;
				}
			}
		}
	}
}
