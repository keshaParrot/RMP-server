using RMP_server.data;
using RMP_server.utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using static RMP_server.utils.NVMLManager;

namespace RMP_server.collectors{
    public class GPUDataCollector : DataCollector {
        private const int NVML_DEVICE_NAME_BUFFER_SIZE = 64;
        private const int NVML_SUCCESS = 0;
        private const int cardNumber = 0;

        public GPUDataCollector() {
            int result = NVMLManager.nvmlInit();
            if (result != NVML_SUCCESS)
            {
                EventLogger.Log($"Failed to initialize NVML: {result}");
                Environment.Exit(-1);
            }
        }

        public override CollectedData CollectData()
        {
            string name = GetDeviceName();
            double load = GetCentralLoad();
            Dictionary<String, long> frequency = GetFrequency();
            double memoryLoad = GetMemoryLoad();
            double memoryTotal = GetTotalMemory();
            double memoryUsed = GetUsedMemory();
            double temperature = GetTemperature();
            double voltage = GetVoltage();
            double fanSpeed = GetFanSpeed();

            return new GPUData(name, load, frequency, memoryLoad, memoryTotal, memoryUsed, temperature, voltage, fanSpeed);
        }

        public void ShutdownNVML()
        {
            nvmlShutdown();
        }

        private IntPtr GetDevice()
        {
            IntPtr device;
            int result = nvmlDeviceGetHandleByIndex((uint)cardNumber, out device);
            if (result != NVML_SUCCESS)
            {
                EventLogger.Log($"Failed to get device handle: {result}");
                Environment.Exit(-1);
            }
            return device;
        }
        private NVMLMemory getNVMLMemory()
        {
            IntPtr device = GetDevice();
            NVMLMemory memoryInfo;
            int result = nvmlDeviceGetMemoryInfo(device, out memoryInfo);
            if (result != NVML_SUCCESS)
            {
                EventLogger.Log($"Failed to get memory info: {result}");
                return memoryInfo;
            }
            return memoryInfo;
        }
        private string GetDeviceName()
        {
            IntPtr device = GetDevice();
            StringBuilder name = new StringBuilder(NVML_DEVICE_NAME_BUFFER_SIZE);
            int result = nvmlDeviceGetName(device, name, NVML_DEVICE_NAME_BUFFER_SIZE);

            if (result != NVML_SUCCESS)
            {
                return "N/A";
            }

            return name.ToString();
        }

        private double GetCentralLoad()
        {
            IntPtr device = GetDevice();
            NVMLUtilization utilization;
            int result = nvmlDeviceGetUtilizationRates(device, out utilization);
            if (result != NVML_SUCCESS)
            {
                EventLogger.Log($"Failed to get GPU utilization: {result}");
                return -1.0;
            }
            return utilization.gpu;
        }

        private Dictionary<String, long> GetFrequency()
        {
            IntPtr device = GetDevice();
            uint smClock, memClock, graphClock, videoClock;
            int result1 = nvmlDeviceGetClockInfo(device, NVMLClockType.NVML_CLOCK_SM, out smClock);
            int result2 = nvmlDeviceGetClockInfo(device, NVMLClockType.NVML_CLOCK_MEM, out memClock);
            int result3 = nvmlDeviceGetClockInfo(device, NVMLClockType.NVML_CLOCK_GRAPHICS, out graphClock);
            int result4 = nvmlDeviceGetClockInfo(device, NVMLClockType.NVML_CLOCK_VIDEO, out videoClock);

            if (result1 != NVML_SUCCESS || result2 != NVML_SUCCESS || result3 != NVML_SUCCESS || result4 != NVML_SUCCESS)
            {
                EventLogger.Log($"Failed to get clock info: {result1}, {result2}, {result3}, {result3}");
                return null;
            }

            var frequencyDict = new Dictionary<string, long>
            {
                { "SM", smClock },
                { "Memory", memClock },
                { "Graphic", graphClock },
                { "Video", videoClock } 
            };

            return frequencyDict;
        }

        private double GetMemoryLoad()
        {
            NVMLMemory memoryInfo = getNVMLMemory();
            return (double)memoryInfo.used / memoryInfo.total.ToInt64() * 100.0;
        }
        private double GetUsedMemory()
        {
            NVMLMemory memoryInfo = getNVMLMemory();
            return (double)memoryInfo.used / 1024 / 1024;
        }

        private double GetTotalMemory()
        {
            NVMLMemory memoryInfo = getNVMLMemory();
            return (double)memoryInfo.total / 1024 / 1024;
        }
        private double GetTemperature()
        {
            IntPtr device = GetDevice();
            uint temperature;
            int result = nvmlDeviceGetTemperature(device, NVMLTemperatureSensor.NVML_TEMPERATURE_GPU, out temperature);
            if (result != NVML_SUCCESS)
            {
                EventLogger.Log($"Failed to get temperature: {result}");
                return -1.0;
            }
            return temperature;
        }

        private double GetVoltage()
        {
            IntPtr device = GetDevice();
            uint voltage;
            int result = nvmlDeviceGetPowerUsage(device, out voltage);
            if (result != NVML_SUCCESS)
            {
                EventLogger.Log($"Failed to get voltage: {result}");
                return -1.0;
            }
            return (double)voltage / 1000.0;
        }

        private double GetFanSpeed()
        {
            IntPtr device = GetDevice();
            uint fanSpeed;
            int result = nvmlDeviceGetFanSpeed(device, out fanSpeed);
            if (result != NVML_SUCCESS)
            {
                EventLogger.Log($"Failed to get fan speed: {result}");
                return -1.0;
            }
            return fanSpeed;
        }
    }
}
