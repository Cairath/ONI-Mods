using Harmony;

namespace NoAutoSave
{
	public class NoAutoSavePatches
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

		[HarmonyPatch(typeof(GameClock))]
		[HarmonyPatch("DoAutoSave")]
		public static class GameClock_DoAutoSave_Patch
		{
			public static bool Prefix()
			{
				return false;
			}
		}
	}
}
