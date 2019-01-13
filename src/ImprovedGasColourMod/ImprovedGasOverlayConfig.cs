using UnityEngine;

namespace ImprovedGasOverlay
{
	public static class ImprovedGasOverlayConfig
	{
		public static readonly Color NotGasColor = new Color(0.6f, 0.6f, 0.6f);
		public const float GasPressureEnd = 2.5f;
		public const float EarPopPressure = 5f;
		public const float MinimumGasColorIntensity = 0.25f;
		public const float FactorValueHSVGases = 0.5f;
		public const float FactorValueHSVCarbonDioxide = 2f;
		public const float SaturationFactor = 1.25f;
	}
}