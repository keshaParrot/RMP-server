using System;
using Newtonsoft.Json;
using System.Collections.Generic;


namespace RMP_server.data
{
    [Serializable]
    public class CoreUnit
    {
        [JsonProperty("name")]
        public string name { get; set; }
        [JsonProperty("load")]
        public Dictionary<String, float> load { get; set; }
        [JsonProperty("temperature")]
        public double temperature { get; set; }
        [JsonProperty("frequency")]
        public float frequency { get; set; }
        [JsonProperty("voltage")]
        public double voltage { get; set; }

        public CoreUnit(string coreName,
                       Dictionary<String, float> coreLoad,
                       double coreTemp,
                       float coreFreq,
                       double coreVoltage)
        {
            name = coreName;
            load = coreLoad;
            temperature = coreTemp;
            frequency = coreFreq;
            voltage = coreVoltage;
        }
        public void printInfo()
        {
            Console.WriteLine("core name {0}", name);

            foreach (var loadUnit in load)
            {
                Console.WriteLine("{0}: {1} %", loadUnit.Key, loadUnit.Value);
            }
            Console.WriteLine("core temperature {0} C", temperature);
            Console.WriteLine("core frequency {0} Hz", frequency);
            Console.WriteLine("core voltage {0} V", voltage);
        }
    }
}