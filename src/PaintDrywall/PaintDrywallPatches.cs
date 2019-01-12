using Harmony;
using UnityEngine;

namespace PaintDrywall
{
	[HarmonyPatch(typeof(BuildingComplete), "OnSpawn")]
	public static class PaintDrywallBuildingCompleteOnSpawnPatch
	{
		public static void Postfix(BuildingComplete __instance)
		{
			if (__instance.name == "ExteriorWallComplete"
				|| __instance.name == "WallpaperComplete"
				|| __instance.name == "ThermalBlockComplete")

			{
				PrimaryElement primaryElement = __instance.GetComponent<PrimaryElement>();
				KAnimControllerBase kAnimBase = __instance.GetComponent<KAnimControllerBase>();

				if (primaryElement != null && kAnimBase != null)
				{
					Color color = primaryElement.Element.substance.colour;
					color.a = 1;

					kAnimBase.TintColour = color;
				}
			}
		}
	}
}
