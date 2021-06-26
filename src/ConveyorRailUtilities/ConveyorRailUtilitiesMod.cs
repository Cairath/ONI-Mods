using HarmonyLib;
using KMod;
using static CaiLib.Logger.Logger;

namespace ConveyorRailUtilities
{
	public class ConveyorRailUtilitiesMod : UserMod2
	{
		public override void OnLoad(Harmony harmony)
		{
			LogInit(mod);

			base.OnLoad(harmony);
		}
	}
}