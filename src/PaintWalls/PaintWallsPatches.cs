using Harmony;
using static CaiLib.Logger.Logger;

namespace PaintWalls
{
	public static class PaintWallPatches
	{
		public static class Mod_OnLoad
		{
			public static void OnLoad()
			{
				LogInit(ModInfo.Name, ModInfo.Version);
			}
		}

		[HarmonyPatch(typeof(BuildingComplete))]
		[HarmonyPatch("OnSpawn")]
		public static class BuildingComplete_OnSpawn_Patch
		{
			public static void Postfix(BuildingComplete __instance)
			{
				if (__instance.name == "ExteriorWallComplete" || __instance.name == "ThermalBlockComplete")
				{
					var primaryElement = __instance.GetComponent<PrimaryElement>();
					var kAnimBase = __instance.GetComponent<KAnimControllerBase>();

					if (primaryElement == null || kAnimBase == null) return;

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
