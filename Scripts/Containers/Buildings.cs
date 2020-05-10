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
        private string scenePath = "res://Scenes/{0}.tscn";

        public Buildings()
        {
            buildings = new BuildingData[Enum.GetNames(typeof(BuildingId)).Length];

            var cityHall = "CityHall";
            buildings[(uint)BuildingId.CityHall] = new BuildingData(cityHall, string.Format(scenePath, cityHall), null, null);

            var recipeHouse = new RecipeData<ResourceId>();
            recipeHouse.Add(ResourceId.Beginnium, 1);
            recipeHouse.Add(ResourceId.Quartz, 1);

            var house = "House";
            buildings[(uint)BuildingId.House] = new BuildingData(house, string.Format(scenePath, house), recipeHouse, null);
        }

        public BuildingData GetBuildingData(BuildingId buildingId)
        {
            return buildings[(uint)buildingId];
        }

        public BuildingNode GetBuildingNode(BuildingId buildingId)
        {
            var buildingData = GetBuildingData(buildingId);
            BuildingNode buildingNode;
            
            switch (buildingId)
            {
                case BuildingId.CityHall:
                    buildingNode = Extensions.SmartSceneLoader<CityHall>(buildingData.ScenePath);
                    break;
                case BuildingId.House:
                    buildingNode = Extensions.SmartSceneLoader<House>(buildingData.ScenePath);
                    break;
                default:
                    throw new InvalidProgramException("No building node recognized.");
            }

            return buildingNode;
        }
    }
}
