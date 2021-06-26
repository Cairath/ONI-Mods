using HarmonyLib;
using KMod;
using static CaiLib.Logger.Logger;

namespace PalmeraTree
{
	public class PalmeraTreeMod : UserMod2
	{
		public override void OnLoad(Harmony harmony)
		{
			LogInit(mod);

			base.OnLoad(harmony);
		}
	}
}