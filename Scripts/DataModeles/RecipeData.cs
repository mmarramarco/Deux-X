using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DeuxX.Scripts
{
    class RecipeData<T>
    {
        protected Dictionary<T, int> data;

        public RecipeData(){
            data = new Dictionary<T, int>();
        }

        public void Add(T key, int quantity)
        {
            if (!data.ContainsKey(key))
            {
                data.Add(key, quantity);
            }
            else
            {
                data[key] = quantity;
            }
        }
    }
}
