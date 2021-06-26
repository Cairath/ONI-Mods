using HarmonyLib;
using KMod;
using static CaiLib.Logger.Logger;

namespace PaintWalls
{
	public class PaintWallsMod : UserMod2
	{
		public override void OnLoad(Harmony harmony)
		{
			LogInit(mod);

			base.OnLoad(harmony);
		}
	}
}