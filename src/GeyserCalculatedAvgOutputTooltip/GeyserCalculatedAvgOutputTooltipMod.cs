using HarmonyLib;
using KMod;
using static CaiLib.Logger.Logger;

namespace GeyserCalculatedAvgOutputTooltip
{
	public class GeyserCalculatedAvgOutputTooltipMod : UserMod2
	{
		public override void OnLoad(Harmony harmony)
		{
			LogInit(mod);

			base.OnLoad(harmony);
		}
	}
}