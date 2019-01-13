using System.Collections.Generic;
using Harmony;

namespace GeyserCalculatedAvgOutputTooltip
{
	public class GeyserCalculatedAvgOutputTooltipPatches
	{
		private static readonly LocString GeyserAvgOutputAnalyse = "Calculated Average Output: (Requires Analysis)";
		private static readonly LocString GeyserAvgOutputAnalyseTooltip = "A researcher must analyze this geyser to determine its average output.";
		private static readonly LocString GeyserAvgOutput = "Calculated Average Output: {0} g/s";
		private static readonly LocString GeyserAvgOutputTooltip = "Taking into account its eruption rates and dormant times, this geyser average output is {0} g/s";

		[HarmonyPatch(typeof(Geyser))]
		[HarmonyPatch("GetDescriptors")]
		public static class Geyser_GetDescriptors_Patch
		{
			public static void Postfix(ref Geyser __instance, ref List<Descriptor> __result)
			{
				const float secondsInCycle = 600f;

				var component = __instance.GetComponent<Studyable>();
				if (component && !component.Studied)
				{
					__result.Add(new Descriptor(GeyserAvgOutputAnalyse, GeyserAvgOutputAnalyseTooltip));
				}
				else
				{
					var emissionRate = __instance.configuration.GetEmitRate() * 1000;
					var eruptionActive = __instance.configuration.GetOnDuration();
					var eruptionTotal = __instance.configuration.GetIterationLength();
					var cyclesActive = __instance.configuration.GetYearOnDuration() / secondsInCycle;
					var cyclesTotal = __instance.configuration.GetYearLength() / secondsInCycle;		
					var avg = (eruptionActive / eruptionTotal) * (cyclesActive / cyclesTotal) * emissionRate;
					var avgStr = avg.ToString("0.00");
					
					__result.Add(new Descriptor(string.Format(GeyserAvgOutput, avgStr), string.Format(GeyserAvgOutputTooltip, avgStr)));
				}
			}
		}
	}
}
