using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DeuxX.Scripts
{
    class Resources
    {
        public static ResourceData[] resources;

        public Resources()
        {
            resources = new ResourceData[Enum.GetNames(typeof(ResourceId)).Length];

            resources[(uint)ResourceId.Beginnium] = new ResourceData("Beginnium");
        }
    }
}
