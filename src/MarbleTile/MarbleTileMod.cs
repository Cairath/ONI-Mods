using HarmonyLib;
using KMod;
using static CaiLib.Logger.Logger;

namespace MarbleTile
{
	public class MarbleTileMod : UserMod2
	{
		public override void OnLoad(Harmony harmony)
		{
			LogInit(mod);

			base.OnLoad(harmony);
		}
	}
}