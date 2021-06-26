using HarmonyLib;

namespace FasterJetpacks
{
	public class FasterJetpacksPatches
	{
		[HarmonyPatch(typeof(BipedTransitionLayer))]
        [HarmonyPatch(nameof(BipedTransitionLayer.BeginTransition))]
		public static class BipedTransitionLayer_BeginTransition_Patch
		{
			public static void Prefix(ref BipedTransitionLayer __instance)
			{
				var instance = Traverse.Create(__instance);

				var floorSpeed = instance.Field("floorSpeed").GetValue<float>();
				var jetpackSpeed = instance.Field("jetPackSpeed");
				jetpackSpeed.SetValue(floorSpeed * FasterJetpacksMod.ConfigManager.Config.SpeedMultiplier);
			}
		}
	}
}
