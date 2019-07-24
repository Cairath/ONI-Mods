using System;

namespace CaiLib.Logger
{
	public static class Logger
	{
		public static void LogInit(string mod, int version)
		{
			LogInit(mod, version.ToString());
		}

		public static void LogInit(string mod, string version)
		{
			Console.WriteLine($"{Timestamp()} <<-- CaiLib -->> Loaded << {mod} >> with version << {version} >>");
		}

		public static void Log(string mod, string message)
		{
			Console.WriteLine($"{Timestamp()} <<-- {mod} -->> " + message);
		}

		private static string Timestamp() => DateTime.UtcNow.ToString("[HH:mm:ss.fff]");
	}
}
