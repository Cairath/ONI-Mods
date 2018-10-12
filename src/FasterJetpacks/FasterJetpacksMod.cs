using Harmony;

namespace FasterJetpacks
{
    public class FasterJetpacksMod
    {
	    [HarmonyPatch(typeof(BipedTransitionLayer))]
		[HarmonyPatch(new [] { typeof(Navigator), typeof(float), typeof(float) })]
	    public static class BipedTransitionLayerPatch
		{
		    public static void Postfix(ref BipedTransitionLayer __instance)
		    {
			    var instance = Traverse.Create(__instance);

				var jetpackSpeed = instance.Field("jetPackSpeed").GetValue<float>();
			    instance.Field("jetPackSpeed").SetValue(jetpackSpeed * 3f);
		    }
	    }
	}
}
