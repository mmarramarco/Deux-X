using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DeuxX.Scripts
{
    class Researches
    {
        public static Research[] data;

        public Researches()
        {
            data = new Research[Enum.GetNames(typeof(ResearchId)).Length];

            Research root = new Research(ResearchId.None, "Root", ResearchId.None, null);

            data[root.id] = root;

            var recip = new RecipeData<ResourceId>();
            recip.add(ResourceId.Beginnium, 1);

            Research res0 = new Research(ResearchId.Res0, "Res0", ResearchId.None, recip);

            data[res0.id] = res0;
        }
    }
}
