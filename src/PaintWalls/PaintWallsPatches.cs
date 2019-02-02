using Harmony;

namespace PaintWalls
{
	public static class PaintWallPatches
	{
		[HarmonyPatch(typeof(SplashMessageScreen))]
		[HarmonyPatch("OnPrefabInit")]
		public static class SplashMessageScreen_OnPrefabInit_Patch
		{
			public static void Postfix()
			{
				CaiLib.ModCounter.ModCounter.Hit(ModInfo.Name, ModInfo.Version);
				CaiLib.Logger.LogInit(ModInfo.Name, ModInfo.Version);
			}
		}

		[HarmonyPatch(typeof(BuildingComplete), "OnSpawn")]
		public static class BuildingComplete_OnSpawn_Patch
		{
			public static void Postfix(BuildingComplete __instance)
			{
				if (__instance.name == "ExteriorWallComplete"
					|| __instance.name == "ThermalBlockComplete")

				{
					var primaryElement = __instance.GetComponent<PrimaryElement>();
					var kAnimBase = __instance.GetComponent<KAnimControllerBase>();

					if (primaryElement != null && kAnimBase != null)
					{
						var element = primaryElement.Element;
						var color = element.substance.uiColour;

						if (element.id == SimHashes.Granite)
						{
							color.a = byte.MaxValue;
						}

						kAnimBase.TintColour = color;
					}
				}
			}
		}
	}
}
