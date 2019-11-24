using Newtonsoft.Json;

namespace LightsOut
{
	public class Config
	{
		[JsonProperty]
		public int LuxThreshold { get; set; } = 1800;

		[JsonProperty]
		public int LowestFog { get; set; } = 30;

		[JsonProperty]
		public int HighestFog { get; set; } = 255;

		[JsonProperty]
		public int LitWorkspaceLux { get; set; } = 500;

		[JsonProperty]
		public int LitDecorLux { get; set; } = 1000;

		[JsonProperty]
		public int DisturbSleepLux { get; set; } = 500;

		[JsonProperty]
		public bool DupeLight { get; set; } = false;

		[JsonProperty]
		public int DupeLightLux { get; set; } = 200;

		[JsonProperty]
		public DebuffTier DebuffTier { get; set; } = DebuffTier.Light;

		[JsonProperty]
		public int PitchBlackLuxThreshold { get; set; } = 0;

		[JsonProperty]
		public int DarkLuxThreshold { get; set; } = 1000;
	}

	public enum DebuffTier
	{
		None,
		Light,
		Harsh
	}
}
