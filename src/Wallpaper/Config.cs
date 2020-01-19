using System.Collections.Generic;
using Newtonsoft.Json;

namespace Wallpaper
{
	public class Config
	{
		[JsonProperty]
		public Dictionary<string, string> Colors { get; set; }
	}
}
