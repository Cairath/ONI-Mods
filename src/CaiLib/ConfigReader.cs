using System;
using System.IO;
using Newtonsoft.Json;

namespace CaiLib
{
	public static class ConfigReader
	{
		public static T ReadConfig<T>(string executingAssemblyPath, string configFileName = "Config.json")
		{
			var directory = Path.GetDirectoryName(executingAssemblyPath);

			if (directory == null)
			{
				return default(T);
			}

			var configPath = Path.Combine(directory, configFileName);

			T config;
			try
			{
				using (var r = new StreamReader(configPath))
				{
					var json = r.ReadToEnd();
					config = JsonConvert.DeserializeObject<T>(json);
				}
			}
			catch (Exception e)
			{
				return default(T);
			}

			return config;
		}
	}
}
