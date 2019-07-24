using Newtonsoft.Json;

namespace FasterJetpacks
{
	public class Config
	{
		[JsonProperty]
		public float SpeedMultiplier { get; set; } = 3;
	}
}
