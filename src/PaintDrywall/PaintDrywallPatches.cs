using Harmony;

namespace PaintDrywall
{
	[HarmonyPatch(typeof(BuildingComplete), "OnSpawn")]
	public static class BuildingComplete_OnSpawn_Patch
	{
		public static void Postfix(BuildingComplete __instance)
		{
			if (__instance.name == "ExteriorWallComplete"
				|| __instance.name == "WallpaperComplete"
				|| __instance.name == "ThermalBlockComplete")

			{
				var primaryElement = __instance.GetComponent<PrimaryElement>();
				var kAnimBase = __instance.GetComponent<KAnimControllerBase>();

				if (primaryElement != null && kAnimBase != null)
				{
					var color = primaryElement.Element.substance.colour;
					color.a = 1;

					kAnimBase.TintColour = color;
				}
			}
		}
	}
}
