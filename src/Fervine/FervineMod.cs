using HarmonyLib;
using KMod;
using static CaiLib.Logger.Logger;

namespace Fervine
{
	public class FervineMod : UserMod2
	{
		public override void OnLoad(Harmony harmony)
		{
			LogInit(mod);

			base.OnLoad(harmony);
		}
	}
}