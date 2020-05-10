using DeuxX.Scripts;
using Godot;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public class BuildingButton : TextureButton
{
    private BuildingId buildingId;
    private const string NormalTexturePath = "res://Ressources/Textures/Default/";
    private const string PressedTexturePath = "res://Ressources/Textures/Pressed/";

    [Signal]
    private delegate void StartBuildingSignal(BuildingId building);

    public void CreateButton(BuildingId buildingId, Node node)
    {
        var name = buildingId.ToString();
        this.buildingId = buildingId;
        CreateButton(name);
        Connect("pressed", this, nameof(OnBuildingButtonClicked));
        Connect(nameof(StartBuildingSignal), node, "StartBuilding");
    }

    public void CreateButton(string name)
    {
        var resourceNormal = GD.Load<Texture>($"{NormalTexturePath}{name}.tres");
        TextureNormal = resourceNormal;
        var resourcePressed = GD.Load<Texture>($"{PressedTexturePath}{name}.tres");
        TexturePressed = resourcePressed;
        Name = name;
    }

    private void OnBuildingButtonClicked()
    {
        EmitSignal(nameof(StartBuildingSignal), buildingId);
    }
}
