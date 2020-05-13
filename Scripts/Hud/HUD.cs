using DeuxX.Scripts;
using Godot;
using System;

public class HUD : Control
{

	private HBoxContainer hBoxContainer;
	private TextureButton upgradeButton;

	[Signal]
	private delegate void SwitchToUpgradeModeSignal(bool toggle);
	[Signal]
	private delegate void StartBuildingSignal(BuildingId buildingId);

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		hBoxContainer = GetNode<HBoxContainer>("HBoxContainer");
		CreateButton("Upgrade");
		upgradeButton = hBoxContainer.GetNode<TextureButton>("Upgrade");
		SetUpUpgradeButton();
	}

	public void Initialize(Node node)
	{
		CreateButton(BuildingId.CityHall, node);
		CreateButton(BuildingId.House, node);
		CreateButton(BuildingId.Steamturbine, node);
		CreateButton(BuildingId.Greenhouse, node);
		CreateButton(BuildingId.Workshop, node);
		CreateButton(BuildingId.Tunnel, node);
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

	private void CreateButton(BuildingId buildingId, Node node)
	{
		var buildingButton = new BuildingButton();
		buildingButton.CreateButton(buildingId, node);
		hBoxContainer.AddChild(buildingButton);
	}

	private void CreateButton(string name)
	{
		var buildingButton = new BuildingButton();
		buildingButton.CreateButton(name);
		hBoxContainer.AddChild(buildingButton);
	}
}
