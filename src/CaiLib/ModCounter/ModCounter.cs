using System;
using System.Net;
using Newtonsoft.Json;

namespace CaiLib.ModCounter
{
	public static class ModCounter
	{
		public static void Hit(string modName, int modVersion)
		{
			var request = new RequestModel
			{
				ApiKey = Config.ApiKey,
				ModName = modName,
				ModVersion = modVersion
			};

			var json = JsonConvert.SerializeObject(request);

			try
			{
				var uri = new Uri(Config.Url);
				var client = new WebClient { Headers = { ["Content-Type"] = "application/json" } };
				client.UploadStringAsync(uri, "POST", json);
			}
			catch (Exception)
			{
				//do nothing
			}
		}
	}
}
