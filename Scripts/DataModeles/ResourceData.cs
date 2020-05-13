using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DeuxX.Scripts
{
    class ResourceData
    {
        string name;

        public uint quantity;

        public int produced;

        public ResourceData(string name)
        {
            this.name = name;
            quantity = 0;
            produced = 0;
        }
    }
}
