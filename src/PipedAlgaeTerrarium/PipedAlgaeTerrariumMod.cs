using HarmonyLib;
using KMod;
using static CaiLib.Logger.Logger;

namespace PipedAlgaeTerrarium
{
	public class PipedAlgaeTerrariumMod : UserMod2
	{
		public override void OnLoad(Harmony harmony)
		{
			LogInit(mod);

			base.OnLoad(harmony);
		}
	}
}