using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RMP_server.data
{
    [Serializable]
    public class CPUData : CollectedData {
        [JsonProperty("name")]
        public string name { get; set; }
        [JsonProperty("centralLoad")]
        public float centralLoad { get; set; }
        [JsonProperty("temperature")]

        public double temperature { get; set; }
        [JsonProperty("coreUnits")]

        public List<CoreUnit> coreUnits;

        public CPUData(string cpuName,
                       float load,
                       double temp,
                       List<CoreUnit> units)
        {
            name = cpuName;
            centralLoad = load;
            temperature = temp;
            coreUnits = units;
        }


        public override void printInfo()
        {
            Console.WriteLine("CPU name {0}", name);
            Console.WriteLine("load {0} %", centralLoad);
            Console.WriteLine("temperature {0} C", temperature);
            foreach (var unit in coreUnits) {
                Console.WriteLine("");
                unit.printInfo();
            }
        }
    }

}
