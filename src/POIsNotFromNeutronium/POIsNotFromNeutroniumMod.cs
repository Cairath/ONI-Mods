using Harmony;
using UnityEngine;

namespace POIsNotFromNeutronium
{
    public class POIsNotFromNeutroniumMod
    {
		[HarmonyPatch(typeof(GeneShufflerConfig), "CreatePrefab")]
		public class GeneShuffler
		{
			private static void Postfix(ref GameObject __result)
			{
				__result.GetComponent<PrimaryElement>().SetElement(SimHashes.Steel);
			}
		}

	    [HarmonyPatch(typeof(PropClockConfig), "CreatePrefab")]
	    public class Clock
	    {
		    private static void Postfix(ref GameObject __result)
		    {
			    __result.GetComponent<PrimaryElement>().SetElement(SimHashes.Polypropylene);
		    }
	    }

		[HarmonyPatch(typeof(SetLockerConfig), "CreatePrefab")]
		public class SetLocker
		{
			private static void Postfix(ref GameObject __result)
			{
				__result.GetComponent<PrimaryElement>().SetElement(SimHashes.Steel);
			}
		}

		[HarmonyPatch(typeof(VendingMachineConfig), "CreatePrefab")]
		public class VendingMachine
		{
			private static void Postfix(ref GameObject __result)
			{
				__result.GetComponent<PrimaryElement>().SetElement(SimHashes.Steel);
			}
		}

		[HarmonyPatch(typeof(PropTableConfig), "CreatePrefab")]
		public class PropTable
		{
			private static void Postfix(ref GameObject __result)
			{
				__result.GetComponent<PrimaryElement>().SetElement(SimHashes.Polypropylene);
			}
		}

		[HarmonyPatch(typeof(LadderPOIConfig), "CreatePrefab")]
		public class LadderPOI
		{
			private static void Postfix(ref GameObject __result)
			{
				__result.GetComponent<PrimaryElement>().SetElement(SimHashes.Steel);
			}
		}
	}
}
