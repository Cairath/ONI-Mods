using System;
using System.Collections.Generic;
using System.Linq;
using Harmony;

namespace Darkness
{
	public class DarknessPatches
	{
		//[HarmonyPatch(typeof(GameInputMapping))]
		//[HarmonyPatch(nameof(GameInputMapping.SetDefaultKeyBindings))]
		//public static class Global_GenerateDefaultBindings_Patch
		//{
		//	public static void Prefix(ref BindingEntry[] default_keybindings)
		//	{
		//		Strings.Add("STRINGS.INPUT_BINDINGS.CAIRATH-DARKNESS.NAME", "Darkness");

		//		var bindings = default_keybindings.ToList();
		//		bindings.Add(new BindingEntry("Cairath-Darkness", GamepadButton.NumButtons, KKeyCode.BackQuote, Modifier.None,
		//			(Action)500));

		//		default_keybindings = bindings.ToArray();
		//	}
		//}



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
				//var flashlight = new List<int>();
				//flashlight.Add(mousePos);
				//var above = Grid.CellAbove(mousePos);
				//var right = Grid.CellRight(mousePos);
				//var below = Grid.CellBelow(mousePos);
				//var left = Grid.CellLeft(mousePos);

				//flashlight.Add(above);
				//flashlight.Add(right);
				//flashlight.Add(left);
				//flashlight.Add(below);
				//flashlight.Add(Grid.CellUpRight(mousePos));
				//flashlight.Add(Grid.CellDownRight(mousePos));
				//flashlight.Add(Grid.CellDownLeft(mousePos));
				//flashlight.Add(Grid.CellUpLeft(mousePos));
				//flashlight.Add(Grid.CellAbove(above));
				//flashlight.Add(Grid.CellBelow(below));
				//flashlight.Add(Grid.CellLeft(left));
				//flashlight.Add(Grid.CellRight(right));

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
						var luxMapped = Math.Min(lux, highestLux);

						//var output = flashlight.Contains(cell) ? 255 : Grid.GetCellDistance()
						//	Remap(luxMapped, lowestLux, highestLux, lowestFog, highestFog);

						var distance = Grid.GetCellDistance(cell, mousePos);
						var output = cell == mousePos ? 255 :
							distance <= 2 ? lowestFog + (255 - lowestFog) * (1f / distance)
							: Remap(luxMapped, lowestLux, highestLux, lowestFog, highestFog);

						region.SetBytes(x, y, (byte)output);
					}
				}

				Debug.Log(DebugHandler.GetMouseCell());

				return false;
			}

			public static float Remap(int value, int from1, int to1, int from2, int to2)
			{
				return (float)(value - from1) / (to1 - from1) * (to2 - from2) + from2;
			}
		}
	}
}
