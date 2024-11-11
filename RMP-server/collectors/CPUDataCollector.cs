using RMP_server.data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Management;
using System.Diagnostics;
using System.Threading;
using LibreHardwareMonitor.Hardware;
using RMP_server.utils;
using System.Text.RegularExpressions;
using System.Runtime.InteropServices;

namespace RMP_server.collectors
{
    public class CPUDataCollector : DataCollector{
        private Computer computer;

        public CPUDataCollector()
        {
            computer = new Computer()
            {
                IsCpuEnabled = true,
                IsGpuEnabled = true,
                IsMemoryEnabled = true,
                IsMotherboardEnabled = true,
                IsControllerEnabled = true,
                IsNetworkEnabled = true,
                IsStorageEnabled = true
            };
            computer.Open();
            computer.Accept(new UpdateVisitor());
        }

        public override CollectedData CollectData()
        {
            string name = GetDeviceName();
            float load = GetDeviceLoad();
            double temp = GetDeviceTemperature();
            List<CoreUnit> coreUnits = packCoreData();

            computer.Close();

            return new CPUData(name, load, temp, coreUnits);
        }

        private List<CoreUnit> packCoreData()
        {
            List<CoreUnit> coreUnits = new List<CoreUnit>();
            List<string> coreNames = extractCoreIdentifier();
            foreach (string coreIdentifier in coreNames)
            {
                string name = coreIdentifier;
                Dictionary<String, float> load = GetCoreLoad(coreIdentifier);
                double temperature = getCoreTemperature(coreIdentifier);
                float frequency = getCoreFrequency(coreIdentifier);
                double voltage = getCoreVoltage(coreIdentifier);
                coreUnits.Add(new CoreUnit(name, load, temperature, frequency, voltage));
            }
            return coreUnits;
        }
        private List<string> extractCoreIdentifier()
        {
            HashSet<string> physicalCores = new HashSet<string>();
            string pattern = @"CPU Core #(\d+)";

            foreach (IHardware hardware in computer.Hardware)
            {
                if (hardware.HardwareType == HardwareType.Cpu)
                {
                    foreach (ISensor sensor in hardware.Sensors)
                    {
                        if (sensor.SensorType == SensorType.Load)
                        {
                            Match match = Regex.Match(sensor.Name, pattern);
                            if (match.Success)
                            {
                                physicalCores.Add(match.Value);
                            }
                        }
                            
                    }
                }
            }
            return new List<string>(physicalCores);
        }
        private string GetDeviceName()
        { 
            foreach (var hardware in computer.Hardware)
            {
                if (hardware.HardwareType == HardwareType.Cpu)
                {
                    return hardware.Name;
                }
            }
            return "Unknown CPU";
        }

        private float GetDeviceLoad()
        {
            float cpuLoad = 0;
            ManagementObjectSearcher searcher = new ManagementObjectSearcher("select * from Win32_Processor");

            foreach (ManagementObject obj in searcher.Get())
            {
                cpuLoad = Convert.ToSingle(obj["LoadPercentage"]);
            }
            return cpuLoad;
        }
        private double GetDeviceTemperature()
        {
            foreach (IHardware hardware in computer.Hardware)
            {
                if (hardware.HardwareType == HardwareType.Cpu)
                {
                    foreach (ISensor sensor in hardware.Sensors)
                    {
                        if (sensor.SensorType == SensorType.Temperature && sensor.Name.Contains("Core Average"))
                            return sensor.Value.GetValueOrDefault();
                    }
                }

            }

            return -1.0;
        }

        private Dictionary<String, float> GetCoreLoad(string coreIdentifier)
        {
            Dictionary<String, float> loads = new Dictionary<string, float>();
            string pattern = @"Thread #\d+";
            foreach (IHardware hardware in computer.Hardware)
            {
                if (hardware.HardwareType == HardwareType.Cpu)
                {
                    foreach (ISensor sensor in hardware.Sensors)
                    {
                        if (sensor.SensorType == SensorType.Load && sensor.Name.Contains(coreIdentifier))
                        {
                            Match match = Regex.Match(sensor.Name, pattern);
                            if (match.Success)
                            {
                                loads.Add(match.Value, sensor.Value.GetValueOrDefault());
                            }
                            else
                            {
                                loads.Add("core load", sensor.Value.GetValueOrDefault());
                            }
                        }
                    }
                }
            }
            return loads;
        }
        private double getCoreTemperature(string coreIdentifier)
        {
            foreach (IHardware hardware in computer.Hardware)
            {
                if (hardware.HardwareType == HardwareType.Cpu)
                {
                    foreach (ISensor sensor in hardware.Sensors)
                    {
                        if (sensor.SensorType == SensorType.Temperature && sensor.Name.Contains(coreIdentifier))
                            return sensor.Value.GetValueOrDefault();
                    }
                }

            }
            return -1.0f;
        }
        private float getCoreFrequency(string coreIdentifier)
        {
            
            foreach (IHardware hardware in computer.Hardware)
            {
                if (hardware.HardwareType == HardwareType.Cpu)
                {
                    foreach (ISensor sensor in hardware.Sensors)
                    {
                        if (sensor.SensorType == SensorType.Clock && sensor.Name.Contains(coreIdentifier))
                        {
                            return sensor.Value.GetValueOrDefault();
                        }
                             
                    }
                }

            }
            return -1.0f;
        }
        private double getCoreVoltage(string coreIdentifier)
        {
            foreach (IHardware hardware in computer.Hardware)
            {
                if (hardware.HardwareType == HardwareType.Cpu)
                {
                    foreach (ISensor sensor in hardware.Sensors)
                    {
                        if (sensor.SensorType == SensorType.Voltage && sensor.Name.Contains(coreIdentifier))
                            return sensor.Value.GetValueOrDefault();
                    }
                }

            }
            return -1.0f;
        }
    }
}
