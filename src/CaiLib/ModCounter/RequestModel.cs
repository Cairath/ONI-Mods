using Newtonsoft.Json;

namespace CaiLib.ModCounter
{
	internal class RequestModel
	{
		[JsonProperty(PropertyName = "apiKey")]
		public string ApiKey { get; set; }

		[JsonProperty(PropertyName = "modName")]
		public string ModName { get; set; }

		[JsonProperty(PropertyName = "modVersion")]
		public int ModVersion { get; set; }
	}
}
