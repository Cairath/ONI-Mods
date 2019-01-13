using Harmony;
using UnityEngine;

namespace RanchingRebalanced.Shinebugs
{
	public static class ShinebugsPatches
	{
		private static readonly Color LightbugColor = new Color(2f, 2f, 0f);
		private static readonly Color LightbugColorOrange = new Color(2f, 1.2f, 0f);
		private static readonly Color LightbugColorPurple = new Color(1.3f, 0.85f, 2.2f);
		private static readonly Color LightbugColorPink = new Color(2f, 1.2f, 2f);
		private static readonly Color LightbugColorBlue = new Color(1f, 1.6f, 2.2f);
		private static readonly Color LightbugColorCrystal = new Color(2f, 2f, 2f);

		[HarmonyPatch(typeof(LightBugConfig))]
		[HarmonyPatch("CreateLightBug")]
		public static class LightBugConfig_CreateLightBug_Patch
		{
			public static void Postfix(ref GameObject __result)
			{
				var light2D = __result.AddOrGet<Light2D>();
				light2D.Color = LightbugColor;
				light2D.Lux = 1800;
				light2D.Range = 5;
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
				light2D.Lux = 3600;
				light2D.Range = 6;
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
				light2D.Lux = 5400;
				light2D.Range = 7;
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
				light2D.Lux = 7200;
				light2D.Range = 8;
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
				light2D.Lux = 10800;
				light2D.Range = 9;
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
				light2D.Lux = 18000;
				light2D.Range = 10;
			}
		}
	}
}
