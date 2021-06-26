using HarmonyLib;
using KMod;
using static CaiLib.Logger.Logger;

namespace ShowIndustrialMachineryTag
{
	public class ShowIndustrialMachineryTagMod : UserMod2
	{
		public override void OnLoad(Harmony harmony)
		{
			LogInit(mod);

			base.OnLoad(harmony);
		}
	}
}