using RMP_server.collectors;
using RMP_server.data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RMP_server.server
{

    public class DataService
    {
        private static CPUDataCollector CPUInstance;
        private static GPUDataCollector GPUInstance;
        private static RAMDataCollector RAMInstance;

        public SystemData packAllData()
        {
            var cpuData = (CPUData)initCpuCollector().CollectData();
            var gpuData = (GPUData)initGpuCollector().CollectData();
            var ramData = (RAMData)initRAMCollector().CollectData();

            SystemData system = new SystemData(cpuData, gpuData, ramData);
            destructCollectors();

            return system;
        }

        private static void destructCollectors()
        {
            CPUInstance = null;
            GPUInstance = null;
            RAMInstance = null;
        }

        private static DataCollector initCpuCollector()
        {
            CPUInstance = new CPUDataCollector();
            return CPUInstance;
        }

        private static DataCollector initGpuCollector()
        {
            GPUInstance = new GPUDataCollector();
            return GPUInstance;
        }

        private static DataCollector initRAMCollector()
        {
            RAMInstance = new RAMDataCollector();
            return RAMInstance;
        }
    }

}
