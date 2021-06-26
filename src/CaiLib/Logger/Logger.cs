using System;
using KMod;

namespace CaiLib.Logger
{
	public static class Logger
	{
		private static Mod _mod;

        public static void LogInit(Mod mod)
        {
	        _mod = mod;

			Console.WriteLine($"{Timestamp()} <<-- CaiLib -->> Loaded [ {mod?.title} ] with version {mod?.packagedModInfo?.version}");
            
		}

		public static void Log(string message)
		{
			if (_mod == null)
			{
				Console.WriteLine($"{Timestamp()} <<-- CaiLib -->> Looks like you have not called LogInit! Please do that before using CaiLib.Log()");
			}

			Console.WriteLine($"{Timestamp()} <<-- {_mod?.title} -->> " + message);
		}

		private static string Timestamp() => System.DateTime.UtcNow.ToString("[HH:mm:ss.fff]");
    }
}
