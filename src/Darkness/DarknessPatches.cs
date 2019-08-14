using System;
using System.Collections.Generic;
using System.Linq;
using Harmony;

namespace Darkness
{
	public class DarknessPatches
	{
		[HarmonyPatch(typeof(GameInputMapping))]
		[HarmonyPatch(nameof(GameInputMapping.SetDefaultKeyBindings))]
		public static class Global_GenerateDefaultBindings_Patch
		{
			public static void Prefix(ref BindingEntry[] default_keybindings)
			{
				Strings.Add("STRINGS.INPUT_BINDINGS.CAIRATH-DARKNESS.NAME", "Darkness");

				var bindings = default_keybindings.ToList();
				bindings.Add(new BindingEntry("Cairath-Darkness", GamepadButton.NumButtons, KKeyCode.BackQuote,
					Modifier.None,
					Action.NumActions));

				default_keybindings = bindings.ToArray();
			}
		}

		[HarmonyPatch(typeof(PropertyTextures))]
		[HarmonyPatch("UpdateFogOfWar")]
		public static class EntityConfigManager_LoadGeneratedEntities_Patch
		{
			public static bool Prefix(TextureRegion region, int x0, int y0, int x1, int y1)
			{
				var lowestLux = 0;
				var highestLux = 1800;

				var lowestFog = 0;
				var highestFog = 255;

				byte[] visible = Grid.Visible;
				var lightIntensityIndexer = Grid.LightIntensity;


				var mousePos = DebugHandler.GetMouseCell();

				for (int y = y0; y <= y1; ++y)
				{
					for (int x = x0; x <= x1; ++x)
					{
						int cell = Grid.XYToCell(x, y);

						if (visible[cell] == 0)
						{
							region.SetBytes(x, y, 0);
							continue;
						}

						var lux = lightIntensityIndexer[cell];

						var distance = Grid.GetCellDistance(cell, mousePos);
						var mouseLight = cell == mousePos ? highestLux :
							distance <= 4 ? highestLux * (1f / distance)
							: 0;

						lux += (int)mouseLight;

						var luxMapped = Math.Min(lux, highestLux);
						var output = Remap(luxMapped, lowestLux, highestLux, lowestFog, highestFog);

						region.SetBytes(x, y, (byte)output);
					}
				}

				return false;
			}

			public static float Remap(int value, int from1, int to1, int from2, int to2)
			{
				return (float)(value - from1) / (to1 - from1) * (to2 - from2) + from2;
			}
		}
	}
}
