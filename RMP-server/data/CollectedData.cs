using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RMP_server.data
{
    [Serializable]
    public abstract class CollectedData
    {
        public abstract void printInfo();
    }
}
