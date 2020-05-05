using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DeuxX.Scripts
{
    class Recipe_Dic<T>
    {
        protected Dictionary<T, int> dic;

        public Recipe_Dic(){
            dic = new Dictionary<T, int>();
        }

        public void add(T key, int quantity)
        {
            if (!dic.ContainsKey(key))
            {
                dic.Add(key, quantity);
            }
            else
            {
                dic[key] = quantity;
            }
        }
    }
}
