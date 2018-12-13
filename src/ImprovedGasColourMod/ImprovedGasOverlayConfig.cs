namespace ImprovedGasColourMod
{
    public static class ImprovedGasOverlayConfig
    {
        public static float GasPressureEnd = 2.5f;
        public static float MinimumGasColorIntensity { get; set; } = 0.25f;
        public static bool ShowEarDrumPopMarker { get; set; } = true;
		public static float FactorValueHSVGases { get; set; } = 0.5f;
		public static float FactorValueHSVCarbonDioxide { get; set; } = 2f;
	}
}