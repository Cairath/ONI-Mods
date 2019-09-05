using System;
using System.IO;
using Newtonsoft.Json;
using static CaiLib.Logger.Logger;

namespace CaiLib.Config
{
	public class ConfigManager<T> where T : class, new()
	{
		public T Config { get; set; }

		private readonly string _executingAssemblyPath;
		private readonly string _configFileName;

		public ConfigManager(string modName, string executingAssemblyPath, string configFileName = "Config.json")
		{
			_executingAssemblyPath = executingAssemblyPath;
			_configFileName = configFileName;
		}

		public T ReadConfig(System.Action postReadAction = null)
		{
			Config = new T();

			var directory = Path.GetDirectoryName(_executingAssemblyPath);

			if (directory == null)
			{
				Log($"Error reading config file {_configFileName} - cannot get directory name for executing assembly path {_executingAssemblyPath}.");
				return Config;
			}

			var configPath = Path.Combine(directory, _configFileName);

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
				Log($"Error reading config file {_configFileName} with exception: {e.Message}");
				return Config;
			}

			Config = config;

			postReadAction?.Invoke();

			return Config;
		}

		public bool SaveConfigToFile()
		{
			var directory = Path.GetDirectoryName(_executingAssemblyPath);

			if (directory == null)
			{
				Log($"Error reading config file {_configFileName} - cannot get directory name for executing assembly path {_executingAssemblyPath}.");
				return false;
			}

			var configPath = Path.Combine(directory, _configFileName);

			try
			{
				using (var r = new StreamWriter(configPath))
				{
					var serialized = JsonConvert.SerializeObject(Config, Formatting.Indented);
					r.Write(serialized);
				}
			}
			catch (Exception e)
			{
				Log($"Error writing to config file {_configFileName} with exception: {e.Message}");
				return false;
			}

			return true;
		}
	}
}
