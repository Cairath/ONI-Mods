using Harmony;
using UnityEngine;

namespace SuitDockStores75Kg
{
    public class SuitDockStores75KgMod
    {
	    [HarmonyPatch(typeof(SuitLockerConfig), "ConfigureBuildingTemplate")]
	    public class SuitLockerConfigPatch
		{
		    private static void Postfix(ref GameObject go)
		    {
				ConduitConsumer conduitConsumer = go.AddOrGet<ConduitConsumer>();
			   
			    conduitConsumer.capacityKG = 75f;
			}
	    }

	    [HarmonyPatch(typeof(JetSuitLockerConfig), "ConfigureBuildingTemplate")]
	    public class JetSuitLockerConfigPatch
		{
		    private static void Postfix(ref GameObject go)
		    {
			    ConduitConsumer conduitConsumer = go.AddOrGet<ConduitConsumer>();

			    conduitConsumer.capacityKG = 75f;
		    }
	    }
	}
}
