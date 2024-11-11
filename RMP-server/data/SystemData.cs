using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RMP_server.data
{
    public class SystemData
    {
        [JsonProperty("Cpu")]
        public CPUData Cpu { get; set; }

        [JsonProperty("Gpu")]
        public GPUData Gpu { get; set; }

        [JsonProperty("Memory")]
        public RAMData Memory { get; set; }
        public SystemData(CPUData cpu,
                        GPUData gpu,
                        RAMData memory) 
        {
            Cpu = cpu;
            Gpu = gpu;
            Memory = memory;
        
        }
        public void printInfo()
        {
            Console.WriteLine("\nCPU");
            Cpu.printInfo();
            Console.WriteLine("\nGPU");
            Gpu.printInfo();
            Console.WriteLine("\nRAM");
            Memory.printInfo();
        }
    }
}
