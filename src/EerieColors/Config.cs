using Newtonsoft.Json;
using UnityEngine;

namespace EerieColors
{
	public class Config
	{
		[JsonProperty]
		public bool CustomBiomeTints { get; set; }

		[JsonProperty]
		public Color32 TintColor { get; set; } = new Color32(190, 80, 200, 0);

		[JsonProperty]
		public bool UnifiedBiomeBackgrounds { get; set; } = true;

		[JsonProperty]
		public int BiomeBackground { get; set; } = 1;

		//public static Color32[] OriginalTintColors { get; } = {
		//    new Color32(145, 198, 213, 0),
		//    new Color32(135, 82, 160, 1),
		//    new Color32(123, 151, 75, 2),
		//    new Color32(236, 189, 89, 3),
		//    new Color32(201, 152, 181, 4),
		//    new Color32(222, 90, 59, 5),
		//    new Color32(201, 152, 181, 6),
		//    new Color32(byte.MaxValue, 0, 0, 7),
		//	  new Color32((byte) 201, (byte) 201, (byte) 151, (byte) 8),
		//	  new Color32((byte) 236, (byte) 90, (byte) 110, (byte) 9),
		//	  new Color32((byte) 110, (byte) 236, (byte) 110, (byte) 10)
		//};
	}
}
