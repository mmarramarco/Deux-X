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
        private readonly string scenePath = "res://Scenes/{0}.tscn";
        private readonly static float OddWide = 8f;
        private readonly static float EvenWide = 0f;
        private readonly static float FourWide = 15f;
        private readonly Vector2 buildingIs3x3 = new Vector2(OddWide, OddWide);
        private readonly Vector2 buildingIs2x3 = new Vector2(EvenWide, OddWide);
        private readonly Vector2 buildingIs3x4 = new Vector2(OddWide, FourWide);
        private readonly Vector2 buildingIs1x2 = new Vector2(OddWide, EvenWide);

        public Buildings()
        {
            buildings = new BuildingData[Enum.GetNames(typeof(BuildingId)).Length];

            var cityHall = "CityHall";
            buildings[(uint)BuildingId.CityHall] = new BuildingData(cityHall, string.Format(scenePath, cityHall), null, null, buildingIs3x3);

            var recipeHouse = new RecipeData<ResourceId>();
            recipeHouse.Add(ResourceId.Beginnium, 1);
            recipeHouse.Add(ResourceId.Quartz, 1);

            var house = "House";
            buildings[(uint)BuildingId.House] = new BuildingData(house, string.Format(scenePath, house), recipeHouse, null, buildingIs2x3);

            var steamTurbine = "Steamturbine";
            buildings[(uint)BuildingId.Steamturbine] = new BuildingData(steamTurbine, string.Format(scenePath, steamTurbine), null, null, buildingIs3x4);

            var greenHouse = "Greenhouse";
            buildings[(uint)BuildingId.Greenhouse] = new BuildingData(greenHouse, string.Format(scenePath, greenHouse), null, null, buildingIs3x3);

            var workShop = "Workshop";
            buildings[(uint)BuildingId.Workshop] = new BuildingData(workShop, string.Format(scenePath, workShop), null, null, buildingIs3x3);

            var tunnel = "Tunnel";
            buildings[(uint)BuildingId.Tunnel] = new BuildingData(tunnel, string.Format(scenePath, tunnel), null, null, buildingIs1x2);
        }

        public BuildingData GetBuildingData(BuildingId buildingId)
        {
            return buildings[(uint)buildingId];
        }

        public BuildingNode GetBuildingNode(BuildingId buildingId)
        {
            var buildingData = GetBuildingData(buildingId);
            BuildingNode buildingNode = null;
            
            switch (buildingId)
            {
                case BuildingId.CityHall:
                    buildingNode = Extensions.SmartSceneLoader<CityHall>(buildingData.ScenePath);
                    break;
                case BuildingId.House:
                    buildingNode = Extensions.SmartSceneLoader<House>(buildingData.ScenePath);
                    break;
                case BuildingId.Steamturbine:
                    buildingNode = Extensions.SmartSceneLoader<Steamturbine>(buildingData.ScenePath);
                    break;
                case BuildingId.Greenhouse:
                    buildingNode = Extensions.SmartSceneLoader<Greenhouse>(buildingData.ScenePath);
                    break;
                case BuildingId.Workshop:
                    buildingNode = Extensions.SmartSceneLoader<Workshop>(buildingData.ScenePath);
                    break;
                case BuildingId.Tunnel:
                    buildingNode = Extensions.SmartSceneLoader<Tunnel>(buildingData.ScenePath);
                    break;
                case BuildingId.Extractor:
                    //buildingNode = Extensions.SmartSceneLoader<Steamturbine>(buildingData.ScenePath);
                    break;
                default:
                    var message = $"If you can read this in the editor, you forgot to add {buildingId} to Buildigns.GetBuildingNode.";
                    GD.Print(message);
                    throw new InvalidProgramException(message);
            }

            if(buildingNode != null)
            {
                buildingNode.Offset = buildingData.Offset;
            }
            
            return buildingNode;
        }
    }
}
