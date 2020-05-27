using Godot;
using Godot.Collections;
using System;
using DeuxX.Scripts;

public class HUD : Control
{

	private VBoxContainer vBoxContainer;
	private TextureButton upgradeButton;

	private Dictionary<ResourceId, ResourceNode> resourceNodes;

	private HBoxContainer hBoxResources;

	[Signal]
	private delegate void SwitchToUpgradeModeSignal(bool toggle);
	[Signal]
	private delegate void StartBuildingSignal(BuildingId buildingId);

	private void addResourceNode(PackedScene ResourcesNode, ResourceId resourceId)
    {
		var node = ResourcesNode.Instance() as ResourceNode;

		node.init(resourceId);

		hBoxResources.AddChild(node);

		resourceNodes.Add(resourceId, node);
	}

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		vBoxContainer = GetNode<VBoxContainer>("VBoxContainer");
		CreateButton("Upgrade");
		upgradeButton = vBoxContainer.GetNode<TextureButton>("Upgrade");
		SetUpUpgradeButton();

		resourceNodes = new Dictionary<ResourceId, ResourceNode>();

		hBoxResources = GetNode<HBoxContainer>("HBoxResources");

		var ResourcesNode = ResourceLoader.Load<PackedScene>("res://Scenes/ResourcesNode.tscn");

		addResourceNode(ResourcesNode, ResourceId.Workers);
		addResourceNode(ResourcesNode, ResourceId.Electricity);
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
        if (resourceNodes.ContainsKey(resourceId))
        {
			resourceNodes[resourceId].reset();
		}
    }
}
