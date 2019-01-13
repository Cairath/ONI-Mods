using UnityEngine;

namespace ImprovedGasOverlay
{
	public static class ColorUtils
	{
		public static ColorHSV GetGasColor(SimHashes elementId, Color primaryColor, float pressureFraction, float mass)
		{
			var color = ScaleColorToPressure(primaryColor.ToHSV(), pressureFraction, elementId);

			if (mass >= ImprovedGasOverlayConfig.EarPopPressure)
			{
				color = MarkEarDrumPopPressure(color, elementId);
			}
			
			return color.Clamp();
		}

		public static ColorHSV ScaleColorToPressure(ColorHSV color, float fraction, SimHashes elementId)
		{
			if (elementId == SimHashes.CarbonDioxide)
			{
				color.V *= (1 - fraction) * ImprovedGasOverlayConfig.FactorValueHSVCarbonDioxide;
			}
			else
			{
				color.S *= fraction * ImprovedGasOverlayConfig.SaturationFactor;
				color.V -= (1 - fraction) * ImprovedGasOverlayConfig.FactorValueHSVGases;
			}

			return color;
		}

		public static Color GetCellOverlayColor(int cellIndex)
		{
			var overlayColor = Grid.Element[cellIndex].substance.colour;
			overlayColor.a = byte.MaxValue;

			return overlayColor;
		}

		public static float GetPressureFraction(float mass, float maxMass)
		{
			return Mathf.Lerp(ImprovedGasOverlayConfig.MinimumGasColorIntensity, 1, mass / maxMass);
		}

		private static ColorHSV MarkEarDrumPopPressure(ColorHSV color, SimHashes elementId)
		{
			if (elementId == SimHashes.CarbonDioxide)
			{
				color.V += 0.3f;
				color.S += 0.4f;
			}
			else
			{
				color.H += 0.1f;
			}

			return color;
		}
	}
}
