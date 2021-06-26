using HarmonyLib;
using KMod;
using static CaiLib.Logger.Logger;

namespace NotificationTrigger
{
	public class NotificationTriggerMod : UserMod2
	{
		public override void OnLoad(Harmony harmony)
		{
			LogInit(mod);

			base.OnLoad(harmony);
		}
	}
}