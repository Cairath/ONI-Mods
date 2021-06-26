using HarmonyLib;
using KMod;
using static CaiLib.Logger.Logger;

namespace MosaicTile
{
	public class MosaicTileMod : UserMod2
	{
		public override void OnLoad(Harmony harmony)
		{
			LogInit(mod);

			base.OnLoad(harmony);
		}
	}
}