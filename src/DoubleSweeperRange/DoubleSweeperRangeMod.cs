using HarmonyLib;
using KMod;
using static CaiLib.Logger.Logger;

namespace DoubleSweeperRange
{
	public class DoubleSweeperRangeMod : UserMod2
	{
		public override void OnLoad(Harmony harmony)
		{
			LogInit(mod);

			base.OnLoad(harmony);
		}
	}
}