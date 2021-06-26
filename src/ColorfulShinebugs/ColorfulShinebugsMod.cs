using HarmonyLib;
using KMod;
using static CaiLib.Logger.Logger;

namespace ColorfulShinebugs
{
	public class ColorfulShinebugsMod : UserMod2
	{
		public override void OnLoad(Harmony harmony)
		{
			LogInit(mod);

			base.OnLoad(harmony);
		}
	}
}