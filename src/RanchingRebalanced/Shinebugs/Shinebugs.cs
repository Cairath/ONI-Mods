using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Harmony;
using TUNING;
using UnityEngine;

namespace RanchingRebalanced
{
	public class Shinebugs
	{
		public static readonly Color LIGHTBUG_COLOR = new Color(2f, 2f, 0f);
		public static readonly Color LIGHTBUG_COLOR_ORANGE = new Color(2f, 1.2f, 0f);
		public static readonly Color LIGHTBUG_COLOR_PURPLE = new Color(1.3f, 0.85f, 2.2f);
		public static readonly Color LIGHTBUG_COLOR_PINK = new Color(2f, 1.2f, 2f);
		public static readonly Color LIGHTBUG_COLOR_BLUE = new Color(1f, 1.6f, 2.2f);
		public static readonly Color LIGHTBUG_COLOR_CRYSTAL = new Color(2f, 2f, 2f);

		[HarmonyPatch(typeof(LightBugConfig), "CreateLightBug")]
		public class CreateLightBugT1
		{
			private static void Postfix(ref GameObject __result)
			{
				Light2D light2D = __result.AddOrGet<Light2D>();
				light2D.Color = LIGHTBUG_COLOR;
				light2D.Lux = 1800;
				light2D.Range = 5;
			}
		}
		

		[HarmonyPatch(typeof(LightBugOrangeConfig), "CreateLightBug")]
		public class CreateLightBugT2
		{
			private static void Postfix(ref GameObject __result)
			{
				Light2D light2D = __result.AddOrGet<Light2D>();
				light2D.Color = LIGHTBUG_COLOR_ORANGE;
				light2D.Lux = 3600;
				light2D.Range = 6;
			}
		}

		[HarmonyPatch(typeof(LightBugPurpleConfig), "CreateLightBug")]
		public class CreateLightBugT3
		{
			private static void Postfix(ref GameObject __result)
			{
				Light2D light2D = __result.AddOrGet<Light2D>();
				light2D.Color = LIGHTBUG_COLOR_PURPLE;
				light2D.Lux = 5400;
				light2D.Range = 7;
			}
		}

		[HarmonyPatch(typeof(LightBugPinkConfig), "CreateLightBug")]
		public class CreateLightBugT4
		{
			private static void Postfix(ref GameObject __result)
			{
				Light2D light2D = __result.AddOrGet<Light2D>();
				light2D.Color = LIGHTBUG_COLOR_PINK;
				light2D.Lux = 7200;
				light2D.Range = 8;
			}
		}

		[HarmonyPatch(typeof(LightBugBlueConfig), "CreateLightBug")]
		public class CreateLightBugT5
		{
			private static void Postfix(ref GameObject __result)
			{
				Light2D light2D = __result.AddOrGet<Light2D>();
				light2D.Color = LIGHTBUG_COLOR_BLUE;
				light2D.Lux = 10800;
				light2D.Range = 9;
			}
		}

		[HarmonyPatch(typeof(LightBugCrystalConfig), "CreateLightBug")]
		public class CreateLightBugT7
		{
			private static void Postfix(ref GameObject __result)
			{
				Light2D light2D = __result.AddOrGet<Light2D>();
				light2D.Color = LIGHTBUG_COLOR_CRYSTAL;
				light2D.Lux = 18000;
				light2D.Range = 10;
			}
		}
	}

}
