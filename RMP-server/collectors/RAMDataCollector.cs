using LibreHardwareMonitor.Hardware;
using RMP_server.data;
using RMP_server.utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace RMP_server.collectors
{
    public class RAMDataCollector : DataCollector
    {
        private Computer computer;
        public RAMDataCollector() {
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
            double load = getMemoryLoad();
            double used = getUsedPhisycal();
            double virtualUsed = getUsedVirtual();
            double available = getAvailablePhisycal();
            double virtualAvailable = getAvailableVirtual();
            double total = used+available;

            computer.Close();

            return new RAMData(load, total, used, virtualUsed, available, virtualAvailable);
        }

        private double getMemoryLoad()
        {
            return getSensorValueByKey("Memory", SensorType.Load);
        }
        private double getUsedPhisycal()
        {
            return getSensorValueByKey("Memory Used", SensorType.Data);
        }

        private double getUsedVirtual()
        {
            return getSensorValueByKey("Virtual Memory Used", SensorType.Data);
        }

        private double getAvailablePhisycal()
        {
            return getSensorValueByKey("Memory Available", SensorType.Data);
        }

        private double getAvailableVirtual()
        {
            return getSensorValueByKey("Virtual Memory Available", SensorType.Data);
        }
        private double getSensorValueByKey(String key, SensorType type)
        {
            foreach (IHardware hardware in computer.Hardware)
            {
                if (hardware.HardwareType == HardwareType.Memory)
                {
                    foreach (ISensor sensor in hardware.Sensors)
                    {
                        if (sensor.SensorType == type && sensor.Name.Equals(key)){
                            return sensor.Value.GetValueOrDefault();
                        }
                    }
                }
            }
            return -1.0;
        }
    }
}
