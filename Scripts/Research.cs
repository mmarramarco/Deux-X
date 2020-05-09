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
        public uint id;

        public string name;

        public uint prev_id;

        public RecipeData<ResourceId> input;

        public bool isResearched;

        public Research(ResearchId id, string name, ResearchId prev_id, RecipeData<ResourceId> input)
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
