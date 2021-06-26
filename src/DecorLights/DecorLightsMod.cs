using HarmonyLib;
using KMod;
using static CaiLib.Logger.Logger;

namespace DecorLights
{
	public class DecorLightsMod : UserMod2
	{
		public override void OnLoad(Harmony harmony)
		{
			LogInit(mod);

			base.OnLoad(harmony);
		}
	}
}