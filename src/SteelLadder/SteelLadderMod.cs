using HarmonyLib;
using KMod;
using static CaiLib.Logger.Logger;

namespace SteelLadder
{
	public class SteelLadderMod : UserMod2
	{
		public override void OnLoad(Harmony harmony)
		{
			LogInit(mod);

			base.OnLoad(harmony);
		}
	}
}