using HarmonyLib;
using KMod;
using static CaiLib.Logger.Logger;

namespace RefinedMetalsUsableAsRawMetals
{
	public class RefinedMetalsUsableAsRawMetalsMod : UserMod2
	{
		public override void OnLoad(Harmony harmony)
		{
			LogInit(mod);

			base.OnLoad(harmony);
		}
	}
}