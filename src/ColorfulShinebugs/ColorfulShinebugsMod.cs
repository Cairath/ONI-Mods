using Harmony;
using UnityEngine;

namespace ColorfulShinebugs
{
    public class ColorfulShinebugsMod
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
			}
		}

		[HarmonyPatch(typeof(LightBugOrangeConfig), "CreateLightBug")]
		public class CreateLightBugT2
		{
			private static void Postfix(ref GameObject __result)
			{
				Light2D light2D = __result.AddOrGet<Light2D>();
				light2D.Color = LIGHTBUG_COLOR_ORANGE;
			}
		}

		[HarmonyPatch(typeof(LightBugPurpleConfig), "CreateLightBug")]
		public class CreateLightBugT3
		{
			private static void Postfix(ref GameObject __result)
			{
				Light2D light2D = __result.AddOrGet<Light2D>();
				light2D.Color = LIGHTBUG_COLOR_PURPLE;
			}
		}

		[HarmonyPatch(typeof(LightBugPinkConfig), "CreateLightBug")]
		public class CreateLightBugT4
		{
			private static void Postfix(ref GameObject __result)
			{
				Light2D light2D = __result.AddOrGet<Light2D>();
				light2D.Color = LIGHTBUG_COLOR_PINK;
			}
		}

		[HarmonyPatch(typeof(LightBugBlueConfig), "CreateLightBug")]
		public class CreateLightBugT5
		{
			private static void Postfix(ref GameObject __result)
			{
				Light2D light2D = __result.AddOrGet<Light2D>();
				light2D.Color = LIGHTBUG_COLOR_BLUE;
			}
		}

		[HarmonyPatch(typeof(LightBugCrystalConfig), "CreateLightBug")]
		public class CreateLightBugT7
		{
			private static void Postfix(ref GameObject __result)
			{
				Light2D light2D = __result.AddOrGet<Light2D>();
				light2D.Color = LIGHTBUG_COLOR_CRYSTAL;
			}
		}
	}
}