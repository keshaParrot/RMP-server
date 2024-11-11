using System;
using System.Runtime.InteropServices;
using System.Text;

namespace RMP_server.utils
{
    public class NVMLManager
    {
        [StructLayout(LayoutKind.Sequential)]
        public struct NVMLUtilization
        {
            public uint gpu;
            public uint memory;
        }

        [StructLayout(LayoutKind.Sequential)]
        public struct NVMLMemory
        {
            public IntPtr total;
            public IntPtr free;
            public IntPtr used;
        }

        public enum NVMLClockType
        {
            NVML_CLOCK_SM,
            NVML_CLOCK_MEM,
            NVML_CLOCK_GRAPHICS,
            NVML_CLOCK_VIDEO
        }

        public enum NVMLTemperatureSensor
        {
            NVML_TEMPERATURE_GPU
        }

        [DllImport("nvml.dll")]
        public static extern int nvmlInit();

        [DllImport("nvml.dll")]
        public static extern int nvmlShutdown();

        [DllImport("nvml.dll")]
        public static extern int nvmlDeviceGetHandleByIndex(uint index, out IntPtr device);

        [DllImport("nvml.dll")]
        public static extern int nvmlDeviceGetName(IntPtr device, StringBuilder name, int length);

        [DllImport("nvml.dll")]
        public static extern int nvmlDeviceGetUtilizationRates(IntPtr device, out NVMLUtilization utilization);

        [DllImport("nvml.dll")]
        public static extern int nvmlDeviceGetClockInfo(IntPtr device, NVMLClockType clockType, out uint clock);

        [DllImport("nvml.dll")]
        public static extern int nvmlDeviceGetMemoryInfo(IntPtr device, out NVMLMemory memory);

        [DllImport("nvml.dll")]
        public static extern int nvmlDeviceGetTemperature(IntPtr device, NVMLTemperatureSensor sensorType, out uint temperature);

        [DllImport("nvml.dll")]
        public static extern int nvmlDeviceGetPowerUsage(IntPtr device, out uint power);

        [DllImport("nvml.dll")]
        public static extern int nvmlDeviceGetFanSpeed(IntPtr device, out uint fanSpeed);
    }
}
