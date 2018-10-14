using System;
using Harmony;

namespace FasterJetpacks
{
    public class FasterJetpacksMod
    {
	    [HarmonyPatch(typeof(BipedTransitionLayer), "BeginTransition")]
	    public static class BipedTransitionLayerPatch
		{
		    public static void Prefix(ref BipedTransitionLayer __instance)
		    {
			    var instance = Traverse.Create(__instance);

				var floorSpeed = instance.Field("floorSpeed").GetValue<float>();
			    var jetpackSpeed = instance.Field("jetPackSpeed").GetValue<float>();

			    if (Math.Abs(floorSpeed - jetpackSpeed) < 0.1f)
				    instance.Field("jetPackSpeed").SetValue(jetpackSpeed * 3f);
			}
	    }
	}
}
