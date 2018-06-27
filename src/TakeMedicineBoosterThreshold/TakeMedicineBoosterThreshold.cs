using Harmony;
using Klei.AI;
using UnityEngine;

namespace TakeMedicineBoosterThreshold
{
	public class TakeMedicineBoosterThreshold
	{
		[HarmonyPatch(typeof(MedicinalPill), "CanBeTakenBy", new[] { typeof(GameObject) })]
		public static class TakeMedicineBoosterThresholdPatch
		{
			public static void Postfix(MedicinalPill __instance, ref GameObject consumer, ref bool __result)
			{
				Effects component = consumer.GetComponent<Effects>();
				if ((Object)component == (Object)null || component.HasEffect(__instance.info.effect))
				{
					__result = false;
					return;
				}

				if (__instance.info.medicineType == MedicineInfo.MedicineType.Booster)
				{
					AmountInstance amountInstance = Db.Get().Amounts.ImmuneLevel.Lookup(consumer);
					if (amountInstance != null)
					{
						__result = (double)amountInstance.value < (double)amountInstance.GetMax() * 0.8f;
						return;
					}
					__result = false;
				}
			}
		}
	}
}
