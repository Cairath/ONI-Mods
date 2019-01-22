using System;
using System.Net;
using Harmony;
using Newtonsoft.Json;

namespace NoAutoSave.ModCounter
{
	[HarmonyPatch(typeof(SplashMessageScreen))]
	[HarmonyPatch("OnPrefabInit")]
	public static class ModCounter
	{
		public static void Postfix()
		{
			var request = new RequestModel
			{
				ApiKey = ModCounterConfig.ApiKey,
				ModName = ModCounterConfig.ModName,
				ModVersion = ModCounterConfig.ModVersion
			};

			var json = JsonConvert.SerializeObject(request);

			try
			{
				var uri = new Uri(ModCounterConfig.Url);
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
