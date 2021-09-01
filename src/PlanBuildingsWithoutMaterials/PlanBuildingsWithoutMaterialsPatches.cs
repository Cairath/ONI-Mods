using HarmonyLib;

namespace PlanBuildingsWithoutMaterials
{
    [HarmonyPatch]
    public class PlanBuildingsWithoutMaterialsPatches
    {
        [HarmonyPatch(typeof(MaterialSelector))]
        [HarmonyPatch("AllowInsufficientMaterialBuild")]
        public static class MaterialSelector_AllowInsufficientMaterialBuild_Patch
        {
            public static bool Prefix(ref bool __result)
            {
                __result = true;
                return false;
            }
        }

        [HarmonyPatch(typeof(PlanScreen))]
        [HarmonyPatch("GetBuildableStateForDef")]
        public static class PlanScreen_GetBuildableStateForDef_Patch
        {
            public static void Postfix(ref PlanScreen.RequirementsState __result)
            {
                if (__result == PlanScreen.RequirementsState.Materials)
                {
                    __result = PlanScreen.RequirementsState.Complete;
                }
            }
        }
    }
}
