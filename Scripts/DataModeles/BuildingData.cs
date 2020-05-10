using Godot;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DeuxX.Scripts
{
    class BuildingData
    {
        readonly string name;
        public readonly string ScenePath;
        readonly RecipeData<ResourceId> recipe;
        readonly ResearchId[] researchNeeded;

        public BuildingData( string name, string scenePath, RecipeData<ResourceId> recipe, ResearchId[] researchNeeded)
        {
            this.name = name;
            ScenePath = scenePath;
            this.recipe = recipe;
            this.researchNeeded = researchNeeded;
        }

        public bool IsBuildable()
        {
            if(recipe == null)
            {
                return true;
            }

            return false;
        }

        public bool IsResearched()
        {
            if (researchNeeded == null)
            {
                return true;
            }

            foreach (var reasearchId in researchNeeded)
            {
                if (!Researches.data[(uint)reasearchId].IsResearched)
                {
                    return false;
                }
            }

            return true;
        }
    }
}
