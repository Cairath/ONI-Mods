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

		[HarmonyPatch(typeof(FacilityBackWallWindowConfig), "CreateBuildingDef")]
		public static class FacilityWindowNotEntombedPatch
		{
			public static void Postfix(ref BuildingDef __result)
			{
				__result.Entombable = false;
			}
		}
	}
}
