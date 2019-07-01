using System;
using Harmony;

namespace FasterJetpacks
{
	public class FasterJetpacksPatches
	{
		[HarmonyPatch(typeof(SplashMessageScreen))]
		[HarmonyPatch("OnPrefabInit")]
		public static class SplashMessageScreen_OnPrefabInit_Patch
		{
			public static void Postfix()
			{
				CaiLib.Logger.LogInit(ModInfo.Name, ModInfo.Version);
			}
		}

		//[HarmonyPatch(typeof(BipedTransitionLayer), "BeginTransition")]
		//public static class BipedTransitionLayerPatch
		//{
		//	public static void Prefix(ref BipedTransitionLayer __instance)
		//	{
		//		var instance = Traverse.Create(__instance);

		//		var floorSpeed = instance.Field("floorSpeed").GetValue<float>();
		//		var jetpackSpeed = instance.Field("jetPackSpeed").GetValue<float>();

		//		if (Math.Abs(floorSpeed - jetpackSpeed) < 0.1f)
		//			instance.Field("jetPackSpeed").SetValue(jetpackSpeed * 3f);
		//	}
		//}

		[HarmonyPatch(typeof(BipedTransitionLayer))]
		[HarmonyPatch(MethodType.Constructor)]
		public static class BipedTransitionLayerPatch
		{
			public static void Postfix(ref float ___jetPackSpeed)
			{
				//var instance = Traverse.Create(__instance);

				//var floorSpeed = instance.Field("floorSpeed").GetValue<float>();
				//var jetpackSpeed = instance.Field("jetPackSpeed").GetValue<float>();

				//if (Math.Abs(floorSpeed - jetpackSpeed) < 0.1f)
				//	instance.Field("jetPackSpeed").SetValue(jetpackSpeed * 3f);
			}
		}
	}
}
