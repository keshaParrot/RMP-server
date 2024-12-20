﻿using Newtonsoft.Json.Linq;
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
            EnsureConfigFileExists();
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
        public static string GetComPort()
        {
            EnsureConfigFileExists();
            var configText = File.ReadAllText(ConfigFileName);
            var config = JObject.Parse(configText);
            return config["COM"]?.ToString();
        }
        public static int GetnIterval()
        {
            EnsureConfigFileExists();
            var configText = File.ReadAllText(ConfigFileName);
            var config = JObject.Parse(configText);
            if (!int.TryParse(config["Interval"]?.ToString(), out int interval))
            {
                EventLogger.Log("Invalid interval value in config file.");
                Environment.Exit(-1);
            }

            return interval * 1000;
        }
        private static void CreateDefaultConfigFile()
        {
            var defaultConfig = new JObject
            {
                ["DataReceptionMode"] = null,
                ["IpAddress"] = null,
                ["COM"] = null,
                ["Interval"] = null
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
