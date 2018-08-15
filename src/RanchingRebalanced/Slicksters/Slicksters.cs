using Harmony;
using STRINGS;
using System.Collections.Generic;
using TUNING;
using UnityEngine;

namespace RanchingRebalanced.Slicksters
{
	public class Slicksters
	{
		[HarmonyPatch(typeof(OilFloaterDecorConfig), "CreateOilFloater")]
		public class LonghairSlicksterConfig
		{
			private static void Postfix(ref GameObject __result)
			{
				var dailyCalories = OilFloaterTuning.STANDARD_CALORIES_PER_CYCLE;

				 var dietList = new List<Diet.Info>();
				dietList.Add(new Diet.Info(new TagBits(SimHashes.Oxygen.CreateTag()), Tag.Invalid, dailyCalories / 20f));

				__result = DietUtils.SetupPooplessDiet(__result, dietList);
			}
		}
	}
}