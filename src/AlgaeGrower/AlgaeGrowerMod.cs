using HarmonyLib;
using KMod;
using static CaiLib.Logger.Logger;

namespace AlgaeGrower
{
	public class AlgaeGrowerMod : UserMod2
	{
		public override void OnLoad(Harmony harmony)
		{
			LogInit(mod);

			base.OnLoad(harmony);
		}
	}
}