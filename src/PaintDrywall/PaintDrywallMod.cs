using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Harmony;
using UnityEngine;

namespace PaintDrywall
{
    [HarmonyPatch(typeof(BuildingComplete), "OnSpawn")]
    public static class BuildingComplete_OnSpawn
    {
        public static void Postfix(BuildingComplete __instance)
        {
            if (__instance.name == "ExteriorWallComplete")
            {
                PrimaryElement primaryElement = __instance.GetComponent<PrimaryElement>();
                KAnimControllerBase kAnimBase = __instance.GetComponent<KAnimControllerBase>();

                if (primaryElement != null && kAnimBase != null)
                {
                    Color color = primaryElement.Element.substance.colour;
                    color.a = 1;

                    kAnimBase.TintColour = color;
                }
            }
        }
    }
}
