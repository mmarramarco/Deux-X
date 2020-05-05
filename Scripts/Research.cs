using Godot;
using Godot.Collections;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DeuxX.Scripts
{
    class Research
    {
        public enum E_Research : uint
        {
            None = 0,
            Res0
        }

        public uint id;

        public string name;

        public uint prev_id;

        public Recipe_Dic<Resources> input;

        public bool isResearched;

        public Research(E_Research id, string name, E_Research prev_id, Recipe_Dic<Resources> input)
        {
            this.id = (uint)id;
            this.name = name;
            this.prev_id = (uint)prev_id;

            if (input != null)
            {
                this.input = input;
                isResearched = false;
            }
            else
            {
                isResearched = true;
            }
        }
    }
}
