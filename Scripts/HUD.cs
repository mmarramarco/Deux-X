using DeuxX.Scripts;
using Godot;
using System;

public class HUD : Control
{

	private HBoxContainer hBoxContainer;
	private const string NormalTexturePath = "res://Ressources/Textures/Default/";
	private const string PressedTexturePath = "res://Ressources/Textures/Pressed/";
	private TextureButton upgradeButton;

	[Signal]
	private delegate void SwitchToUpgradeModeSignal(bool toggle);

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		hBoxContainer = GetNode<HBoxContainer>("HBoxContainer");
		CreateButton("Upgrade");
		CreateButton(BuildingId.CityHall);
		CreateButton(BuildingId.Unknown);
		upgradeButton = hBoxContainer.GetNode<TextureButton>("Upgrade");
		SetUpUpgradeButton();
	}

	private void SetUpUpgradeButton()
	{
		upgradeButton.ToggleMode = true;
		upgradeButton.Connect("toggled", this, nameof(OnUpgradeToggle));
	}

	private void OnUpgradeToggle(bool toggle)
	{
		GD.Print($"toggled {toggle}");
		EmitSignal(nameof(SwitchToUpgradeModeSignal), toggle);
	}

	private void CreateButton(BuildingId building)
	{
		var name = building.ToString();
		CreateButton(name);
	}

	private void CreateButton(string name)
	{
		var button = new TextureButton();
		var resourceNormal = GD.Load<Texture>($"{NormalTexturePath}{name}.tres");
		button.TextureNormal = resourceNormal;
		var resourcePressed = GD.Load<Texture>($"{PressedTexturePath}{name}.tres");
		button.TexturePressed = resourcePressed;
		button.Name = name;
		hBoxContainer.AddChild(button);
	}
}
