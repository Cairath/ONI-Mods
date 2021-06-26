using HarmonyLib;
using KMod;
using static CaiLib.Logger.Logger;

namespace NoLongCommutes
{
	public class NoLongCommutesMod : UserMod2
	{
		public override void OnLoad(Harmony harmony)
		{
			LogInit(mod);

			base.OnLoad(harmony);
		}
	}
}