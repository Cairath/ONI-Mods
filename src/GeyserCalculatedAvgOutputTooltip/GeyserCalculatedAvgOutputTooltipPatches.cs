using System.Collections.Generic;
using Harmony;
using static CaiLib.Logger.Logger;

namespace GeyserCalculatedAvgOutputTooltip
{
	public class GeyserCalculatedAvgOutputTooltipPatches
	{
		private static readonly LocString GeyserAvgOutputAnalyse = "Calculated Average Output: (Requires Analysis)";
		private static readonly LocString GeyserAvgOutputAnalyseTooltip = "A researcher must analyze this geyser to determine its average output.";
		private static readonly LocString GeyserAvgOutput = "Calculated Average Output: {0} {1}";
		private static readonly LocString GeyserAvgOutputTooltip = "Taking into account its eruption rates and dormant times, this geyser average output is {0} {1}";

		[HarmonyPatch(typeof(SplashMessageScreen))]
		[HarmonyPatch("OnPrefabInit")]
		public static class SplashMessageScreen_OnPrefabInit_Patch
		{
			public static void Postfix()
			{
				LogInit(ModInfo.Name, ModInfo.Version);
			}
		}

		[HarmonyPatch(typeof(Geyser))]
		[HarmonyPatch(nameof(Geyser.GetDescriptors))]
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

					var units = "g/s";
					if (avg > 1000)
					{
						avg /= 1000;
						units = "kg/s";
					}
					
					var avgStr = avg.ToString("0.00");
					
					__result.Add(new Descriptor(string.Format(GeyserAvgOutput, avgStr, units), string.Format(GeyserAvgOutputTooltip, avgStr, units)));
				}
			}
		}
	}
}
