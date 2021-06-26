using HarmonyLib;
using KMod;
using static CaiLib.Logger.Logger;

namespace SuitDockStores75Kg
{
	public class SuitDockStores75KgMod : UserMod2
	{
		public override void OnLoad(Harmony harmony)
		{
			LogInit(mod);

			base.OnLoad(harmony);
		}
	}
}