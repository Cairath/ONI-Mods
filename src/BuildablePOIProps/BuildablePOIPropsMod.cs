using HarmonyLib;
using KMod;
using static CaiLib.Logger.Logger;

namespace BuildablePOIProps
{
	public class BuildablePOIPropsMod : UserMod2
	{
		public override void OnLoad(Harmony harmony)
		{
			LogInit(mod);

			base.OnLoad(harmony);
		}
	}
}