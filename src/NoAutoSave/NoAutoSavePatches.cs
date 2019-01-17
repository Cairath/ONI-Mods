using Harmony;

namespace NoAutoSave
{
	public class NoAutoSavePatches
	{
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
