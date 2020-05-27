using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DeuxX.Scripts
{
    class Resources
    {
        public static ResourceData[] data;

        public Resources()
        {
            var names = Enum.GetNames(typeof(ResourceId));

            var length = names.Length;

            data = new ResourceData[length];

            for(uint i = 0; i<length; i++)
            {
                data[i] = new ResourceData(names[i]);
            }
        }
    }
}
