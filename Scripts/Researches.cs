using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DeuxX.Scripts
{
    class Researches
    {
        public Research[] researches;

        public Researches()
        {
            researches = new Research[Enum.GetNames(typeof(Research.E_Research)).Length];

            Research root = new Research(Research.E_Research.None, "Root", Research.E_Research.None, null);

            researches[root.id] = root;

            var recip = new Recipe_Dic<Resources>();
            recip.add(Resources.Beginnium, 1);

            Research res0 = new Research(Research.E_Research.Res0, "Res0", Research.E_Research.None, recip);

            researches[res0.id] = res0;
        }
    }
}
