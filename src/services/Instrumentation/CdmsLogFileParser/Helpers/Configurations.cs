﻿using System.Configuration;
using System.IO;
using System.Reflection;

namespace CdmsLogFileParser.Helpers
{
    public static class Configurations
    {
        public static string FileExtension;
        public static string LogsPath;

        static Configurations()
        {
            FileExtension = GetStringConfiguration(Constants.ConfigKey_FileExtension, defaultVal: "*.log");
            LogsPath = GetStringConfiguration(Constants.ConfigKey_LogsPath, defaultVal: Constants.DefaultLogsPath);
        }

        #region Helpers

        public static string GetRelativeOrPhysicalPath(string configKey, string defaultValue = null)
        {
            string pathToLogFiles = GetStringConfiguration(configKey);
            if (string.IsNullOrEmpty(pathToLogFiles))
                pathToLogFiles = defaultValue;

            if (!pathToLogFiles.Contains(":"))
            {
                string localPath = Assembly.GetExecutingAssembly().Location;

                localPath = Path.GetDirectoryName(localPath);
                pathToLogFiles = string.Format(@"{0}\{1}", localPath, pathToLogFiles);
            }

            return pathToLogFiles;
        }

        public static string GetStringConfiguration(string key, string defaultVal = null, bool allowNull = false)
        {
            string val = null;
            val = ConfigurationManager.AppSettings[key];
            if (string.IsNullOrEmpty(val))
                val = defaultVal;

            if (string.IsNullOrEmpty(val) && allowNull == false)
            {
                string msg = string.Format(
                    "No value for the key: '{0}' was found in {1}, nor was a default value supplied.", key, "config file appSettings");

                throw new ConfigurationErrorsException(msg);
            }
            return val;
        }

        public static int GetIntConfiguration(string key, int defaultVal)
        {
            string val = ConfigurationManager.AppSettings[key];
            if (string.IsNullOrEmpty(val))
                return defaultVal;

            int valInt;
            if (!int.TryParse(val, out valInt))
                throw new ConfigurationErrorsException(string.Format("Config file Value for the key: '{0}' was expected to be of type int. Actual: {1}", key, val));

            return valInt;
        }

        public static bool GetBoolConfiguration(string key, bool defaultVal = false)
        {
            string val = ConfigurationManager.AppSettings[key];
            if (string.IsNullOrEmpty(val))
                return defaultVal;

            bool valBool;
            if (!bool.TryParse(val, out valBool))
                throw new ConfigurationErrorsException(string.Format("Config file Value for the key: '{0}' was expected to be of type bool. Actual: {1}", key, val));

            return valBool;
        }

        #endregion
    }
}
