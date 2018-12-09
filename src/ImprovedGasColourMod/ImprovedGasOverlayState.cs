namespace ImprovedGasColourMod
{
    public static class ImprovedGasOverlayState
    {
        public static bool Enabled { get; set; } = true;

        public static float GasPressureEnd = 2.5f;

        // not used anymore
        public static float GasPressureStart { get; set; } = 0.1f;
        
        // gas overlay
        public static float MinimumGasColorIntensity { get; set; } = 0.25f;

        // major fps drop when enabled
        public static bool AdvancedGasOverlayDebugging { get; set; } = false;

        public static bool ShowEarDrumPopMarker { get; set; } = true;

		public static float FactorValueHSVGases { get; set; } = 0.5f;

		public static float FactorValueHSVCarbonDioxide { get; set; } = 2f;
		
	}
}