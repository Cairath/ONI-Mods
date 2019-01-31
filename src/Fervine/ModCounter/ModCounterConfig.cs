using Newtonsoft.Json;

namespace Fervine.ModCounter
{
	public static class ModCounterConfig
	{
		public static string Url = "http://modstats.toolsnotincluded.net/api/hits/hit";
		public static string ApiKey = "80b9a48acad34a8694d940e938e75dfb";
		public static string ModName = "Fervine";
		public static int ModVersion = 2;
	}

	class RequestModel
	{
		[JsonProperty(PropertyName = "apiKey")]
		public string ApiKey { get; set; }

		[JsonProperty(PropertyName = "modName")]
		public string ModName { get; set; }

		[JsonProperty(PropertyName = "modVersion")]
		public int ModVersion { get; set; }
	}
}
