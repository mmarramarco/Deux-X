using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DeuxX.Scripts
{
    class Recipe<T, I>
    {
        public Recipe_Dic<T> input;

        public Recipe_Dic<I> output;
    }
}
