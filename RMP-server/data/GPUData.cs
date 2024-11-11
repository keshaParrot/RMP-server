using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace RMP_server.data
{
    [Serializable]
    public class GPUData : CollectedData{
        [JsonProperty("name")]
        public string name { get; set; }
        [JsonProperty("load")]
        public double load { get; set; }
        [JsonProperty("frequency")]
        public long[] frequency { get; set; }
        [JsonProperty("memoryLoad")]
        public double memoryLoad { get; set; }
        [JsonProperty("temperature")]
        public double temperature { get; set; }
        [JsonProperty("voltage")]
        public double voltage { get; set; }
        [JsonProperty("fanSpeed")]
        public double fanSpeed { get; set; }

        public GPUData(string name, double load, long[] frequency, double memoryLoad, double temperature, double voltage, double fanSpeed)
        {
            this.name = name;
            this.load = load;
            this.frequency = frequency;
            this.memoryLoad = memoryLoad;
            this.temperature = temperature;
            this.voltage = voltage;
            this.fanSpeed = fanSpeed;
        }

        public override void printInfo()
        {
            Console.WriteLine("GPU Name: {0}", name);
            Console.WriteLine("GPU Load: {0}%", load);

            Console.Write("Frequencies: ");
            for (int i = 0; i < frequency.Length; ++i)
            {
                Console.Write("{0} MHz ", frequency[i]);
            }
            Console.WriteLine();

            Console.WriteLine("Memory Load: {0}%", memoryLoad);
            Console.WriteLine("Temperature: {0} °C", temperature);
            Console.WriteLine("Voltage: {0} V", voltage);
            Console.WriteLine("Fan Speed: {0} RPM", fanSpeed);
        }
    }
}
