using Harmony;

namespace DrawBuildingsWithoutMaterials
{
	public class DrawBuildingsWithoutMaterialsMod
	{
		[HarmonyPatch(typeof(MaterialSelector), "AllowInsufficientMaterialBuild")]
		public class MaterialSelectorPatch
		{
			private static bool Prefix(ref bool __result)
			{
				__result = true;
				return false;
			}
		}
	}
}
