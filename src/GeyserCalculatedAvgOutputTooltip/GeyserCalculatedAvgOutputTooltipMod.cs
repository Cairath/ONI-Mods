using System;
using System.Collections.Generic;
using Harmony;
using UnityEngine;

namespace GeyserCalculatedAvgOutputTooltip
{
	public class GeyserCalculatedAvgOutputTooltipMod
	{
		public static LocString GeyserAvgOutputAnalyse = "Calculated Average Output: (Requires Analysis)";
		public static LocString GeyserAvgOutputAnalyseTooltip = "A researcher must analyze this geyser to determine its average output.";
		public static LocString GeyserAvgOutput = "Calculated Average Output: {0} g/s";
		public static LocString GeyserAvgOutputTooltip = "Taking into account its eruption rates and dormant times, this geyser average output is {0} g/s";

		[HarmonyPatch(typeof(Geyser), "GetDescriptors")]
		public class ModifierSetPatch
		{
			private static void Postfix(ref Geyser __instance, GameObject go, ref List<Descriptor> __result)
			{
				Studyable component = __instance.GetComponent<Studyable>();
				if (component && !component.Studied)
				{
					__result.Add(new Descriptor(GeyserAvgOutputAnalyse, GeyserAvgOutputAnalyseTooltip));
				}
				else
				{
					var emissionRate = __instance.configuration.GetEmitRate() * 1000;
					var eruptionActive = __instance.configuration.GetOnDuration();
					var eruptionTotal = __instance.configuration.GetIterationLength();
					var cyclesActive = __instance.configuration.GetYearOnDuration() / 600;
					var cyclesTotal = __instance.configuration.GetYearLength() / 600;		
					var avg = (eruptionActive / eruptionTotal) * (cyclesActive / cyclesTotal) * emissionRate;
					var avgStr = avg.ToString("0.00");
					
					__result.Add(new Descriptor(String.Format(GeyserAvgOutput, avgStr), String.Format(GeyserAvgOutputTooltip, avgStr)));
				}
			}
		}
	}
}
