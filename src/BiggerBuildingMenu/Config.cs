using Newtonsoft.Json;

namespace BiggerBuildingMenu
{
	public class Config
	{
		[JsonProperty]
		public int Height { get; set; } = 8;
	}
}
