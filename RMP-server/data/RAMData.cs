using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RMP_server.data
{
    [Serializable]
    public class RAMData : CollectedData
    {
        [JsonProperty("load")]
        public double load { get; set; }
        [JsonProperty("total")]
        public double total { get; set; }
        [JsonProperty("used")]
        public double used { get; set; }
        [JsonProperty("virtualUsed")]
        public double virtualUsed { get; set; }
        [JsonProperty("available")]
        public double available { get; set; }
        [JsonProperty("virtualAvailable")]
        public double virtualAvailable { get; set; }

        public RAMData(double mload, 
                    double mtotal, 
                    double mused, 
                    double mvirtualUsed, 
                    double mavailable, 
                    double mvirtualAvailable)
        {
            load = mload;
            total = mtotal;
            used = mused;
            virtualUsed = mvirtualUsed;
            available = mavailable;
            virtualAvailable = mvirtualAvailable;
        }

        public override void printInfo()
        {
            Console.WriteLine("Load: {0} %", load);
            Console.WriteLine("Total Memory: {0} GB",total);
            Console.WriteLine("Used Memory: {0} GB", used);
            Console.WriteLine("Virtual Used Memory: {0} GB", virtualUsed);
            Console.WriteLine("Available Memory: {0} GB", available);
            Console.WriteLine("Virtual Available Memory: {0} GB",virtualAvailable);
        }
    }
    
}
