using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Reflection;
using Instrumentation.WebApp.Models;
using WebGrease.Css.Extensions;

namespace Instrumentation.WebApp.Helpers
{
    public class Configurations
    {
        public static string ReleaseVersion { get; private set; }
        public static string DbKeys { get; private set; }
        public static string DbKeyDefault { get; private set; }
        public static int MaxRowCountDefault { get; private set; }

        static Configurations()
        {
            ReleaseVersion = GetStringConfiguration(Constants.ConfigKey_ReleaseVersion, allowNull: true);
            DbKeys = GetStringConfiguration(Constants.ConfigKey_DbKeys);
            MaxRowCountDefault = GetIntConfiguration(Constants.ConfigKey_MaxRowCount, Constants.DefaultMaxRowCount);
            DbKeyDefault = GetStringConfiguration(Constants.ConfigKey_DbKeyDefault);
        }

        public static Dictionary<string, Dictionary<string, string>> GetConfigurationItems(Dictionary<string, Dictionary<string, string>> configurationItems = null)
        {
            configurationItems = configurationItems ?? new Dictionary<string, Dictionary<string, string>>();

            var itemGroup = new Dictionary<string, string>();
            configurationItems.Add("Instrumentation Configurations", itemGroup);
            itemGroup.Add("ReleaseVersion", ReleaseVersion);

            return configurationItems;
        }

        public static List<LookupItem> GetDbKeysFromConfig()
        {
            var dbOptions = new List<LookupItem>();

            var keys = DbKeys;
            var splits = keys.Split(';');
            if (splits == null || splits.Length == 0)
                throw new ConfigurationErrorsException("DBKeys configuration should have 1 or more dbkeys.");

            splits.ForEach(split => dbOptions.Add(new LookupItem { Value = split, Description = split }));

            return dbOptions;
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

        public static string GetConnectionString(string key, string defaultVal = null, bool allowNull = false)
        {
            if (ConfigurationManager.ConnectionStrings[key] == null)
                throw new ConfigurationErrorsException(string.Format("connection String not found for key: {0}", key));

            string val = null;
            val = ConfigurationManager.ConnectionStrings[key].ConnectionString;
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

        #endregion
    }
}