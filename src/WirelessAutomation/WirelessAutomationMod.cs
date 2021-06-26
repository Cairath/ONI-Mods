using HarmonyLib;
using KMod;
using static CaiLib.Logger.Logger;

namespace WirelessAutomation
{
	public class WirelessAutomationMod : UserMod2
	{
		public override void OnLoad(Harmony harmony)
		{
			LogInit(mod);

			base.OnLoad(harmony);
		}
	}
}