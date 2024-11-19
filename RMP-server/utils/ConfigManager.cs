using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RMP_server.utils
{
    public static class ConfigManager
    {
        private const string ConfigFileName = "appsettings.json";

        public static string GetDataReceptionMode()
        {
            if (!File.Exists(ConfigFileName))
            {
                CreateDefaultConfigFile();
            }

            var configText = File.ReadAllText(ConfigFileName);
            var config = JObject.Parse(configText);
            return config["DataReceptionMode"]?.ToString();
        }
        public static string GetIpAddress()
        {
            EnsureConfigFileExists();
            var configText = File.ReadAllText(ConfigFileName);
            var config = JObject.Parse(configText);
            return config["IpAddress"]?.ToString();
        }
        private static void CreateDefaultConfigFile()
        {
            var defaultConfig = new JObject
            {
                ["DataReceptionMode"] = null,
                ["IpAddress"] = null
            };

            var jsonConfig = JsonConvert.SerializeObject(defaultConfig, Formatting.Indented);

            File.WriteAllText(ConfigFileName, jsonConfig);

            EventLogger.Log($"file {ConfigFileName} was created with default values .");
        }
        private static void EnsureConfigFileExists()
        {
            if (!File.Exists(ConfigFileName))
            {
                CreateDefaultConfigFile();
            }
        }
    }
}
