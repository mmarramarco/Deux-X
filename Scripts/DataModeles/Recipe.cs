using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DeuxX.Scripts
{
    class Recipe<T, I>
    {
        public RecipeData<T> Input;

        public RecipeData<I> Output;
    }
}
