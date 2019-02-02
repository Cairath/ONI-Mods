using System;
using System.IO;
using System.Reflection;
using Newtonsoft.Json;
using UnityEngine;

namespace EerieColors
{
	public class Config
	{
		[JsonProperty]
		public static bool CustomBiomeTints { get; set; } = true;

		[JsonProperty]
		public static Color32 TintColor { get; set; } = new Color32(190, 80, 200, 0);

		[JsonProperty]
		public static bool Darken { get; set; } = true;

		[JsonProperty]
		public static bool UnifiedBiomeBackgrounds { get; set; } = true;

		[JsonProperty]
		public static int BiomeBackground { get; set; } = 1;

		//public static Color32[] OriginalTintColors { get; } = {
		//    new Color32(145, 198, 213, 0),
		//    new Color32(135, 82, 160, 1),
		//    new Color32(123, 151, 75, 2),
		//    new Color32(236, 189, 89, 3),
		//    new Color32(201, 152, 181, 4),
		//    new Color32(222, 90, 59, 5),
		//    new Color32(201, 152, 181, 6),
		//    new Color32(byte.MaxValue, 0, 0, 7)
		//};

		public static void InitConfig()
		{
			CaiLib.ConfigReader.ReadConfig<Config>(Assembly.GetExecutingAssembly().Location);		
			MathUtil.Clamp(BiomeBackground, 0, 6);
		}
	}
}
