using HarmonyLib;
using KMod;
using static CaiLib.Logger.Logger;

namespace AchievementProgress
{
	public class AchievementProgressMod : UserMod2
	{
		public override void OnLoad(Harmony harmony)
		{
			LogInit(mod);
			base.OnLoad(harmony);
		}
	}
}