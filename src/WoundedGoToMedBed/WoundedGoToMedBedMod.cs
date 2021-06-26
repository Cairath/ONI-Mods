using HarmonyLib;
using KMod;
using static CaiLib.Logger.Logger;

namespace WoundedGoToMedBed
{
	public class WoundedGoToMedBedMod : UserMod2
	{
		public override void OnLoad(Harmony harmony)
		{
			LogInit(mod);

			base.OnLoad(harmony);
		}
	}
}