using HarmonyLib;
using KMod;
using static CaiLib.Logger.Logger;

namespace DebugDoesNotDiscoverMap
{
	public class DebugDoesNotDiscoverMapMod : UserMod2
	{
		public override void OnLoad(Harmony harmony)
		{
			LogInit(mod);

			base.OnLoad(harmony);
		}
	}
}