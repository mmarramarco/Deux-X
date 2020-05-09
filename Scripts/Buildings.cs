using Godot;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DeuxX.Scripts
{
    class Buildings
    {
        public static BuildingData[] buildings;

        public Buildings()
        {
            buildings = new BuildingData[Enum.GetNames(typeof(BuildingId)).Length];

            buildings[(uint)BuildingId.CityHall] = new BuildingData("Cityhall", "res://", null, null);

            var recipeHouse = new RecipeData<ResourceId>();
            recipeHouse.add(ResourceId.Beginnium, 1);
            recipeHouse.add(ResourceId.Quartz, 1);

            buildings[(uint)BuildingId.House] = new BuildingData("House", "res://", recipeHouse, null);
        }
    }
}
