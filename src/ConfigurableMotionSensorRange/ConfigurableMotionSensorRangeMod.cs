using HarmonyLib;
using KMod;
using static CaiLib.Logger.Logger;

namespace ConfigurableMotionSensorRange
{
	public class ConfigurableMotionSensorRangeMod : UserMod2
	{
		public override void OnLoad(Harmony harmony)
		{
			LogInit(mod);

			base.OnLoad(harmony);
		}
	}
}