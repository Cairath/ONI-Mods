using Harmony;
using UnityEngine;

namespace DoubleSweeperRange
{
    public class DoubleSweeperRangeMod
    {
	    [HarmonyPatch(typeof(SolidTransferArmConfig), "DoPostConfigurePreview")]
	    public class DoPostConfigurePreviewPatch
		{
		    private static void Postfix(ref GameObject go)
		    {
			    go.GetComponent<StationaryChoreRangeVisualizer>().range = 8;
			}
	    }

		[HarmonyPatch(typeof(SolidTransferArmConfig), "DoPostConfigureComplete")]
	    public class DoPostConfigureCompletePatch
		{
		    private static void Postfix(ref GameObject go)
		    {
			    go.GetComponent<StationaryChoreRangeVisualizer>().range = 8;
			    go.GetComponent<SolidTransferArm>().pickupRange = 8;
			}
	    }
	}
}
