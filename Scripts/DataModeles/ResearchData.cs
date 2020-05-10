using Godot;
using Godot.Collections;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DeuxX.Scripts
{
    class ResearchData
    {
        public uint Id;

        public string Name;

        public uint PreviousId;

        public RecipeData<ResourceId> Input;

        public bool IsResearched;

        public ResearchData(ResearchId id, string name, ResearchId previousId, RecipeData<ResourceId> input)
        {
            this.Id = (uint)id;
            this.Name = name;
            this.PreviousId = (uint)previousId;

            if (input != null)
            {
                this.Input = input;
                IsResearched = false;
            }
            else
            {
                IsResearched = true;
            }
        }
    }
}
