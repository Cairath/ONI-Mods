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
			var location = Assembly.GetExecutingAssembly().Location;
			var directory = Path.GetDirectoryName(location);

			if (directory == null)
			{
				return;
			}

			var configFileName = "Config.json";
			var configPath = Path.Combine(directory, configFileName);

			try
			{
				using (var r = new StreamReader(configPath))
				{
					var json = r.ReadToEnd();
					JsonConvert.DeserializeObject<Config>(json);
				}
			}
			catch (Exception e)
			{
				return;
			}

			Clamp(BiomeBackground, 0, 6);
		}

		public static T Clamp<T>(T value, T min, T max) where T : IComparable<T>
		{
			T result = value;
			if (value.CompareTo(max) > 0)
				result = max;
			if (value.CompareTo(min) < 0)
				result = min;
			return result;
		}
	}
}