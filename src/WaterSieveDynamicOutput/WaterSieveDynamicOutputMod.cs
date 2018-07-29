using Harmony;
using UnityEngine;

namespace WaterSieveFixedOutput
{
    public class WaterSieveDynamicOutputMod
    {
	    [HarmonyPatch(typeof(WaterPurifierConfig), "ConfigureBuildingTemplate")]
	    public static class WaterSieveDynamicOutputPatch
		{
		    public static void Postfix(WaterPurifierConfig __instance, ref GameObject go)
		    {
				ElementConverter elementConverter = go.AddOrGet<ElementConverter>();

			    var newOutputElements = new[]
			    {
				    new ElementConverter.OutputElement(5f, SimHashes.Water, 0f, true, 0f, 0.5f, true, 0.75f, 255, 0),
				    new ElementConverter.OutputElement(0.2f, SimHashes.ToxicSand, 0f, true, 0f, 0.5f, true, 0.25f, 255, 0)
			    };

			    elementConverter.outputElements = newOutputElements;
			}
	    }
	}
}
