using System;
using System.IO;
using KMod;
using Newtonsoft.Json;
using static CaiLib.Logger.Logger;

namespace CaiLib.Config
{
	public class ConfigManager<T> where T : class, new()
	{
		public T Config { get; set; }

		private readonly string _modPath;
		private readonly string _configFileName;

		public ConfigManager(Mod mod, string configFileName = "Config.json")
		{
			_modPath = mod.ContentPath;
			_configFileName = configFileName;
		}

		public T ReadConfig(System.Action postReadAction = null)
		{
			Config = new T();
			
			var configPath = Path.Combine(_modPath, _configFileName);
			Debug.Log(configPath);
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
				Log($"Failed to read config file {_configFileName} with exception: {e.Message}");
				return Config;
			}

			Config = config;

			postReadAction?.Invoke();

			return Config;
		}

		public bool SaveConfigToFile()
		{
			var directory = Path.GetDirectoryName(_modPath);

			if (directory == null)
			{
				Log($"Failed to read file {_configFileName} - cannot get directory name for executing assembly path {_modPath}.");
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
				Log($"Failed to write to config file {_configFileName} with exception: {e.Message}");
				return false;
			}

			return true;
		}
	}
}
