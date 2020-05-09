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
        string name;
        string scenePath;
        RecipeData<ResourceId> recipe;
        ResearchId[] researchNeeded;

        public BuildingData(string name, string scenePath, RecipeData<ResourceId> recipe, ResearchId[] researchNeeded)
        {
            this.name = name;
            this.scenePath = scenePath;
            this.recipe = recipe;
            this.researchNeeded = researchNeeded;
        }

        public bool isBuildable()
        {
            if(recipe == null)
            {
                return true;
            }

            return false;
        }

        public bool isResearched()
        {
            if (researchNeeded == null){
                return true;
            }

            foreach (var reasearchId in researchNeeded){
                if (!Researches.data[(uint)reasearchId].isResearched)
                {
                    return false;
                }
            }

            return true;
        }
    }
}
