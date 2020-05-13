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

        /// <summary>
        /// The position offset to build this building into the world.
        /// </summary>
        public Vector2 Offset { get; internal set; }

        /// <summary>
        /// The building data view modele. It holds every necessary data for a specific building.
        /// </summary>
        /// <param name="name">The building's name.</param>
        /// <param name="scenePath">The building's scene path.</param>
        /// <param name="recipe">The recipe to craft this building.</param>
        /// <param name="researchNeeded">The research needed to be able to build this building.</param>
        /// <param name="offset">The position offset of the building.</param>
        public BuildingData( string name, string scenePath, RecipeData<ResourceId> recipe, ResearchId[] researchNeeded, Vector2 offset)
        {
            this.name = name;
            ScenePath = scenePath;
            this.recipe = recipe;
            this.researchNeeded = researchNeeded;
            Offset = offset;
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

        /// <summary>
        /// Mostly for debugging purposes...
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return $"Name : {name}, scenepath : {ScenePath}";
        }
    }
}
