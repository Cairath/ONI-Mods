using System;
using System.Linq;
using System.Reflection;

namespace CaiLib.Logger
{
	public static class Logger
    {
        private static string _modName = string.Empty;

        public static void LogInit()
        {
			Console.WriteLine($"{Timestamp()} <<-- CaiLib -->> Loaded [ {GetModName()} ] with version {Assembly.GetExecutingAssembly().GetName().Version}");
		}

		public static void Log(string message)
		{
			Console.WriteLine($"{Timestamp()} <<-- {GetModName()} -->> " + message);
		}

		private static string Timestamp() => System.DateTime.UtcNow.ToString("[HH:mm:ss.fff]");

        private static string GetModName()
        {
            if (Logger._modName != string.Empty)
                return Logger._modName;

            var assembly = Assembly.GetExecutingAssembly();
            var assemblyName = assembly.GetName().Name;
            var modInfo = assembly.GetExportedTypes().FirstOrDefault(p => p.GetInterfaces().Contains(typeof(IModInfo)));

            if (modInfo == null) return assemblyName;

            var modInfoInstance = Activator.CreateInstance(modInfo);
            var modName = modInfo.GetProperty(nameof(IModInfo.Name))?.GetValue(modInfoInstance, null);


            Logger._modName = modName == null ? assemblyName : modName.ToString();
            return Logger._modName;
        }
    }
}
