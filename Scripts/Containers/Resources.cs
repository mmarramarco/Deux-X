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
            data = new ResourceData[Enum.GetNames(typeof(ResourceId)).Length];

            data[(uint)ResourceId.Workers] = new ResourceData("Workers");

            data[(uint)ResourceId.Beginium] = new ResourceData("Beginium");
        }
    }
}
