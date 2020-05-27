using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DeuxX.Scripts
{
    class ResourceData
    {
        public string name;

        public uint quantity;

        public int produced;

        public ResourceData(string name)
        {
            this.name = name;
            quantity = 0;
            produced = 0;
        }

        public string[] getValues()
        {
            string[] s = new string[2];

            s[0] = String.Format("{0:D6}", quantity);

            if (produced >= 0){
                s[1] += "+" + String.Format("{0:D3}", produced);
            }
            else
            {
                s[1] += produced.ToString();
            }

            return s;
        }
    }
}
