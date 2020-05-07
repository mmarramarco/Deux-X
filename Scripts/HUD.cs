using Godot;
using System;

public class HUD : Control
{

    private HBoxContainer hBoxContainer;
    private const string NormalTexturePath = "res://Ressources/Textures/Default/";
    private const string PressedTexturePath = "res://Ressources/Textures/Pressed/";

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        hBoxContainer = GetNode<HBoxContainer>("HBoxContainer");
        CreateButton(BuildingIds.CityHall);
        CreateButton(BuildingIds.Unknown);
    }

//  // Called every frame. 'delta' is the elapsed time since the previous frame.
//  public override void _Process(float delta)
//  {
//      
//  }

    private void CreateButton(BuildingIds building)
    {
        var button = new TextureButton();
        var name = building.ToString();
        var resourceNormal = GD.Load<Texture>($"{NormalTexturePath}{name}.tres");
        button.TextureNormal = resourceNormal;
        var resourcePressed = GD.Load<Texture>($"{PressedTexturePath}{name}.tres");
        button.TexturePressed = resourcePressed;

        hBoxContainer.AddChild(button);
    }
}
