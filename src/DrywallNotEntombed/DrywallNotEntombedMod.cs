using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Reflection.Emit;
using Harmony;

namespace DrywallNotEntombed
{
	public class DrywallNotEntombedMod
	{
		[HarmonyPatch(typeof(ExteriorWallConfig), "CreateBuildingDef")]
		public static class DrywallNotEntombedPatch
		{
			public static void Postfix(ref BuildingDef __result)
			{
				__result.Entombable = false;
			}
		}
	}
}
