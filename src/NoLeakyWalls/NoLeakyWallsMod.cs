using HarmonyLib;
using KMod;
using static CaiLib.Logger.Logger;

namespace NoLeakyWalls
{
	public class NoLeakyWallsMod : UserMod2
	{
		public override void OnLoad(Harmony harmony)
		{
			LogInit(mod);

			base.OnLoad(harmony);
		}
	}
}