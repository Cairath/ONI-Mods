using HarmonyLib;
using KMod;
using static CaiLib.Logger.Logger;

namespace DebugDoesNotDisableAchievements
{
	public class DebugDoesNotDisableAchievementsMod : UserMod2
	{
		public override void OnLoad(Harmony harmony)
		{
			LogInit(mod);

			base.OnLoad(harmony);
		}
	}
}