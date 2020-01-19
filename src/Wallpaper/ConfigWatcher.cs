using System.IO;
using System.Reflection;

namespace Wallpaper
{
	public class ConfigWatcher
	{
		public delegate void Callback();

		private FileSystemWatcher _fileWatcher;
		private Callback _callback;

		public ConfigWatcher(Callback onChanged)
		{
			_callback = onChanged;

			var dir = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
			CaiLib.Logger.Logger.Log(dir);
			_fileWatcher = new FileSystemWatcher(dir, "Config.json");

			_fileWatcher.Changed += _fileWatcher_Changed;
			_fileWatcher.Created += _fileWatcher_Changed;
			_fileWatcher.Renamed += _fileWatcher_Changed;
			_fileWatcher.Deleted += _fileWatcher_Changed;

			_fileWatcher.EnableRaisingEvents = true;
		}

		private void _fileWatcher_Changed(object sender, FileSystemEventArgs e)
		{
			_callback();
		}
	}
}