using RMP_server.data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace RMP_server.collectors
{
    public abstract class DataCollector
    {
        public abstract CollectedData CollectData();


    }
}
