using Harmony;
using UnityEngine;

namespace ColorfulShinebugs
{
    public class ColorfulShinebugsPatches
    {
	    private static readonly Color LightbugColor = new Color(2f, 2f, 0f);
	    private static readonly Color LightbugColorOrange = new Color(2f, 1.2f, 0f);
	    private static readonly Color LightbugColorPurple = new Color(1.3f, 0.85f, 2.2f);
	    private static readonly Color LightbugColorPink = new Color(2f, 1.2f, 2f);
	    private static readonly Color LightbugColorBlue = new Color(1f, 1.6f, 2.2f);
	    private static readonly Color LightbugColorCrystal = new Color(2f, 2f, 2f);

	    [HarmonyPatch(typeof(SplashMessageScreen))]
	    [HarmonyPatch("OnPrefabInit")]
	    public static class SplashMessageScreen_OnPrefabInit_Patch
	    {
		    public static void Postfix()
		    {
			    CaiLib.Logger.Logger.LogInit(ModInfo.Name, ModInfo.Version);
		    }
	    }

		[HarmonyPatch(typeof(LightBugConfig))]
		[HarmonyPatch("CreateLightBug")]
		public static class LightBugConfig_CreateLightBug_Patch
		{
			public static void Postfix(ref GameObject __result)
			{
				var light2D = __result.AddOrGet<Light2D>();
				light2D.Color = LightbugColor;
			}
		}

		[HarmonyPatch(typeof(LightBugOrangeConfig))]
		[HarmonyPatch("CreateLightBug")]
		public static class LightBugOrangeConfig_CreateLightBug_Patch
		{
			public static void Postfix(ref GameObject __result)
			{
				var light2D = __result.AddOrGet<Light2D>();
				light2D.Color = LightbugColorOrange;
			}
		}

		[HarmonyPatch(typeof(LightBugPurpleConfig))]
		[HarmonyPatch("CreateLightBug")]
		public class LightBugPurpleConfig_CreateLightBug_Patch
		{
			public static void Postfix(ref GameObject __result)
			{
				var light2D = __result.AddOrGet<Light2D>();
				light2D.Color = LightbugColorPurple;
			}
		}

		[HarmonyPatch(typeof(LightBugPinkConfig))]
		[HarmonyPatch("CreateLightBug")]
		public static class LightBugPinkConfig_CreateLightBug_Patch
		{
			public static void Postfix(ref GameObject __result)
			{
				var light2D = __result.AddOrGet<Light2D>();
				light2D.Color = LightbugColorPink;
			}
		}

		[HarmonyPatch(typeof(LightBugBlueConfig))]
		[HarmonyPatch("CreateLightBug")]
		public static class LightBugBlueConfig_CreateLightBug_Patch
		{
			public static void Postfix(ref GameObject __result)
			{
				var light2D = __result.AddOrGet<Light2D>();
				light2D.Color = LightbugColorBlue;
			}
		}

		[HarmonyPatch(typeof(LightBugCrystalConfig))]
		[HarmonyPatch("CreateLightBug")]
		public static class LightBugCrystalConfig_CreateLightBug_Patch
		{
			public static void Postfix(ref GameObject __result)
			{
				var light2D = __result.AddOrGet<Light2D>();
				light2D.Color = LightbugColorCrystal;
			}
		}
	}
}