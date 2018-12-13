using Harmony;
using UnityEngine;

namespace ImprovedGasColourMod
{
   
    public static class HarmonyPatches
    {
        private static readonly Color NotGasColor = new Color(0.6f, 0.6f, 0.6f);

        [HarmonyPatch(typeof(SimDebugView), "GetOxygenMapColour")]
        public static class ImprovedGasOverlayMod
        {
            public const float EarPopFloat = 5;

            public static bool Prefix(int cell, ref Color __result)
            {
                //  ModSettings settings = ONI_Common.ModdyMcModscreen
                //float maxMass = StateManager.ConfiguratorState.GasPressureEnd;
                float maxMass = ImprovedGasOverlayState.GasPressureEnd;

                Element element = Grid.Element[cell];

                if (!element.IsGas)
                {
                    __result = NotGasColor;
                    return false;
                }

                float mass = Grid.Mass[cell];
                SimHashes elementID = element.id;
                Color primaryColor = GetCellOverlayColor(cell);
                float pressureFraction = GetPressureFraction(mass, maxMass);

                __result = GetGasColor(elementID, primaryColor, pressureFraction, mass);

                return false;
            }

            private static ColorHSV GetGasColor(SimHashes elementID, Color primaryColor, float pressureFraction, float mass)
            {
                ColorHSV colorHSV = primaryColor.ToHSV();

                colorHSV = ScaleColorToPressure(colorHSV, pressureFraction, elementID);

                //if (StateManager.ConfiguratorState.ShowEarDrumPopMarker)
                if (ImprovedGasOverlayState.ShowEarDrumPopMarker)
                {
                    colorHSV = MarkEarDrumPopPressure(colorHSV, mass, elementID);
                }

                //if (StateManager.ConfiguratorState.AdvancedGasOverlayDebugging)
                if (ImprovedGasOverlayState.AdvancedGasOverlayDebugging)
                {
                    colorHSV.CheckAndLogOverflow(elementID, pressureFraction);
                }

                colorHSV = colorHSV.Clamp();

                return colorHSV;
            }

            private static ColorHSV ScaleColorToPressure(ColorHSV color, float fraction, SimHashes elementID)
            {
                if (elementID == SimHashes.CarbonDioxide)
                {
					//color.V *= (1 - fraction) * 2;
					color.V *= (1 - fraction) * ImprovedGasOverlayState.FactorValueHSVCarbonDioxide;
				}
                else
                {
                    color.S *= fraction * 1.25f;
					//color.V -= (1 - fraction) / 2;
					color.V -= (1 - fraction) * ImprovedGasOverlayState.FactorValueHSVGases;
				}

                return color;
            }

            public static Color GetCellOverlayColor(int cellIndex)
            {
                Element element = Grid.Element[cellIndex];
                Substance substance = element.substance;

                Color32 overlayColor = substance.overlayColour;

                overlayColor.a = byte.MaxValue;

                return overlayColor;
            }

            private static float GetPressureFraction(float mass, float maxMass)
            {
                //float minFraction = StateManager.ConfiguratorState.MinimumGasColorIntensity;
                float minFraction = ImprovedGasOverlayState.MinimumGasColorIntensity;

                float fraction = mass / maxMass;

                fraction = Mathf.Lerp(minFraction, 1, fraction);

                return fraction;
            }

            /// <summary>
            /// Add flat value to color hue when pressure reaches EarPopFloat
            /// </summary>
            private static ColorHSV MarkEarDrumPopPressure(ColorHSV color, float mass, SimHashes elementID)
            {
                if (mass > EarPopFloat)
                {
                    if (elementID == SimHashes.CarbonDioxide)
                    {
                        color.V += 0.3f;
                        color.S += 0.4f;
                    }
                    else
                    {
                        // TODO: make hue change customizable in config
                        color.H += 0.1f;
                    }
                }

                return color;
            }
        }
    }
}