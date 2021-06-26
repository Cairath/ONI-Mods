using HarmonyLib;
using KMod;
using static CaiLib.Logger.Logger;

namespace OilWellAnyWater
{
	public class OilWellAnyWaterMod : UserMod2
	{
		public override void OnLoad(Harmony harmony)
		{
			LogInit(mod);

			base.OnLoad(harmony);
		}
	}
}