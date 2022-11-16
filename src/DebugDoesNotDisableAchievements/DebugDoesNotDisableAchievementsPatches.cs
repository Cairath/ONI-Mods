using Database;
using HarmonyLib;

namespace DebugDoesNotDisableAchievements
{
	public class DebugDoesNotDisableAchievementsPatches
	{
		[HarmonyPatch(typeof(ColonyAchievementTracker))]
		[HarmonyPatch("UnlockPlatformAchievement")]
		public static class ColonyAchievementTracker_UnlockPlatformAchievement_Patch
		{
			public static bool Prefix(string achievement_id)
			{
				if (DebugHandler.InstantBuildMode)
				{
					Debug.LogWarningFormat("UnlockPlatformAchievement {0} skipping: instant build mode", (object)achievement_id);
				}
				else if (SaveGame.Instance.sandboxEnabled)
				{
					Debug.LogWarningFormat("UnlockPlatformAchievement {0} skipping: sandbox mode", (object)achievement_id);
				}
				//else if (Game.Instance.debugWasUsed)
				//{
				//	Debug.LogWarningFormat("UnlockPlatformAchievement {0} skipping: debug was used.", (object)achievement_id);
				//}
				else
				{
					ColonyAchievement colonyAchievement = Db.Get().ColonyAchievements.Get(achievement_id);
					if (colonyAchievement == null || string.IsNullOrEmpty(colonyAchievement.platformAchievementId))
					{
						return false;
					}

					if ((bool)SteamAchievementService.Instance)
					{
						SteamAchievementService.Instance.Unlock(colonyAchievement.platformAchievementId);
					}
					else
					{
						Debug.LogWarningFormat("Steam achievement [{0}] was achieved, but achievement service was null",
							(object)colonyAchievement.platformAchievementId);
					}
				}

				return false;
			}
		}
	}
}
