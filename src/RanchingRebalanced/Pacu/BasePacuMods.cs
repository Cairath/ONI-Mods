using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Harmony;
using UnityEngine;

namespace RanchingRebalanced.Pacu
{
	class BasePacuMods
	{
		[HarmonyPatch(typeof(BasePacuConfig), "CreatePrefab")]
		public class CreatePrefab
		{
			private static void Postfix(ref GameObject __result)
			{
				__result.AddOrGet<OutOfLiquidMonitor>();
			}
		}
	}
}