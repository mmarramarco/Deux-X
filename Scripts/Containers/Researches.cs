using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DeuxX.Scripts
{
    class Researches
    {
        public static ResearchData[] data;

        public Researches()
        {
            data = new ResearchData[Enum.GetNames(typeof(ResearchId)).Length];

            ResearchData root = new ResearchData(ResearchId.None, "Root", ResearchId.None, null);

            data[root.Id] = root;

            var recip = new RecipeData<ResourceId>();
            recip.Add(ResourceId.Beginium, 1);

            ResearchData res0 = new ResearchData(ResearchId.Res0, "Res0", ResearchId.None, recip);

            data[res0.Id] = res0;
        }
    }
}
