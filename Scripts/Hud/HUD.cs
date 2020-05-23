using Godot;
using Godot.Collections;
using System;
using DeuxX.Scripts;

public class HUD : Control
{

	private VBoxContainer vBoxContainer;
	private TextureButton upgradeButton;

	private Dictionary<ResourceId, ResourceNode> resourceNodes;

	[Signal]
	private delegate void SwitchToUpgradeModeSignal(bool toggle);
	[Signal]
	private delegate void StartBuildingSignal(BuildingId buildingId);

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		vBoxContainer = GetNode<VBoxContainer>("VBoxContainer");
		CreateButton("Upgrade");
		upgradeButton = vBoxContainer.GetNode<TextureButton>("Upgrade");
		SetUpUpgradeButton();

		resourceNodes = new Dictionary<ResourceId, ResourceNode>();

		var ResourcesNode = ResourceLoader.Load<PackedScene>("res://Scenes/ResourcesNode.tscn");

		var hBoxResources = GetNode<HBoxContainer>("HBoxResources");

		var node = ResourcesNode.Instance() as ResourceNode;

		node.init(ResourceId.Workers);

		hBoxResources.AddChild(node);

		resourceNodes.Add(ResourceId.Workers, node);
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
		vBoxContainer.AddChild(buildingButton);
	}

	private void CreateButton(string name)
	{
		var buildingButton = new BuildingButton();
		buildingButton.CreateButton(name);
		vBoxContainer.AddChild(buildingButton);
	}

	public void resetResource(ResourceId resourceId)
    {
		resourceNodes[resourceId].reset();
    }
}
