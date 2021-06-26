using HarmonyLib;
using KMod;
using static CaiLib.Logger.Logger;

namespace LessWasteFromJetpacks
{
	public class LessWasteFromJetpacksMod : UserMod2
	{
		public override void OnLoad(Harmony harmony)
		{
			LogInit(mod);

			base.OnLoad(harmony);
		}
	}
}