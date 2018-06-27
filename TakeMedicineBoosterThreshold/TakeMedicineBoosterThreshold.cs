using Harmony;
using Klei.AI;
using UnityEngine;

namespace TakeMedicineBoosterThresholdMod
{
	public class TakeMedicineBoosterThreshold
	{
		[HarmonyPatch(typeof(MedicinalPill), "CanBeTakenBy", new[] { typeof(GameObject) })]
		public static class TakeMedicineBoosterThresholdPatch
		{
			public static void Postfix(MedicinalPill __instance, ref GameObject consumer, ref bool __result)
			{
				if (__instance.info.medicineType == MedicineInfo.MedicineType.Booster)
				{
					AmountInstance amountInstance = Db.Get().Amounts.ImmuneLevel.Lookup(consumer);
					if (amountInstance != null)
					{
						__result = (double)amountInstance.value < (double)amountInstance.GetMax() * 0.5f;
					}
					else
					{
						__result = false;
					}

				}

			}
		}
	}
}
