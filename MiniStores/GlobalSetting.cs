﻿namespace MiniStores
{
    public static class GlobalSetting
    {
        // Properties ONLY
        public static bool SettingChanged = true;

        public static bool DebugApp = false;
        public static string Language = "";
        private static int logsToKeep = 2;

        /// <summary>
        /// Sets up the Global Setting File
        /// </summary>
        public static void InitiliasSettings()
        {
            LoadSettings();
        }
        // ***************************
        /// <summary>
        /// Get\Set's the Logs to Keep Property
        /// </summary>
        public static int LogsToKeep
        {
            get
            {
                var MyConfig = new IniFile("MiniStores.ini");
                logsToKeep = int.Parse(MyConfig.Read("LogsToKeep"));

                return logsToKeep;
            }

            set
            {
                logsToKeep = value;
            }
        }
        // ***************************
        /// <summary>
        /// Load's the Setting from File
        /// </summary>
        public static void LoadSettings()
        {
            var MyConfig = new IniFile("MiniStores.ini");

            // Get the Debug Setting
            if (!MyConfig.KeyExists("DebugApp"))
            {
                MyConfig.Write("DebugApp", DebugApp.ToString());
            }
            else
            {
                DebugApp = bool.Parse(MyConfig.Read("DebugApp"));
            }

            // Get the Language Setting
            if (!MyConfig.KeyExists("Language"))
            {
                MyConfig.Write("Language", Language);
            }
            else
            {
                Language = MyConfig.Read("Language");
            }

            // Get the LogsToKeep Setting
            if (!MyConfig.KeyExists("LogsToKeep"))
            {
                MyConfig.Write("LogsToKeep", logsToKeep.ToString());
            }
            else
            {
                logsToKeep = int.Parse(MyConfig.Read("LogsToKeep"));
            }

        }
        // ***************************
        /// <summary>
        /// Save's the Setting to a File
        /// </summary>
        public static void SaveSettings()
        {
            var MyConfig = new IniFile("MiniStores.ini");

            MyConfig.Write("DebugApp", DebugApp.ToString());
            MyConfig.Write("Language", Language);
            MyConfig.Write("LogsToKeep", logsToKeep.ToString());
        }
    }
}