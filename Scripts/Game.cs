using Godot;
using System;
using DeuxX.Scripts;
using System.Collections.Generic;

public class Game : Node
{
    [Signal]
    private delegate void ShowBuildingSignal(bool show);

    private Buildings buildings = new Buildings();
    private Resources resources = new Resources();

    public List<BuildingNode> buildingNodes = new List<BuildingNode>();

    private Camera2D camera;
    private BuildingNode buildingToPlace = null;
    private bool canBuildHere = true;
    private List<int> collidingWithAreaId = new List<int>();
    private ManagementMode currentMode = ManagementMode.CameraHandlingMode;
    private HUD hud;

	private World world;

	private YSort ysort;

	public override void _Ready()
    {
		camera = GetNode<Camera2D>("Camera2D");
		world = GetNode<World>("World");
		hud = GetNode<HUD>("CanvasLayer/HUD");
		ysort = GetNode<YSort>("YSort");

		world.GenerateStartingIsland();
        
        hud.Connect("SwitchToUpgradeModeSignal", this, nameof(SwitchToUpgradeMode));
        hud.Initialize(this);
    }

	public override void _Process(float delta)
	{
		if (Input.IsActionJustPressed("ui_select"))
		{
			world.GenerateStartingIsland();
		}

		if (Input.IsActionJustPressed("tab_key"))
		{
			EmitSignal(nameof(ShowBuildingSignal), false);
		}
		if (Input.IsActionJustReleased("tab_key"))
		{
			EmitSignal(nameof(ShowBuildingSignal), true);
		}

		switch (currentMode)
		{
			case ManagementMode.BuildingMode:
				BuildingMode();
				break;
			default:
				break;
		}

		if (Input.IsActionJustPressed("ui_right_click"))
		{
			ResetBuildingMode(removeChild: true);
		}
	}

	private void StartBuilding(BuildingId buildingId)
	{
		buildingToPlace = Buildings.data[(uint)buildingId].scene.Instance() as BuildingNode;
		buildingToPlace.Initialize();
		ysort.AddChild(buildingToPlace);
		currentMode = ManagementMode.BuildingMode;
	}

	private void SwitchToUpgradeMode(bool toggle)
	{
		foreach (var building in buildingNodes)
		{
			building.SwitchToUpgradeMode(toggle);
		}
	}
	private void BuildingMode()
	{
		var viewport = GetViewport();
		var position = (viewport.GetMousePosition() - viewport.Size / 2) * camera.Zoom + camera.Position;
		var tilePosition = world.WorldToMap(position);
		var realPosition = world.MapToWorld(tilePosition) + buildingToPlace.Offset; // building offset, TODO : make it prettier, and per building.
		buildingToPlace.Position = realPosition;

		if (canBuildHere)
		{
			if (Input.IsActionJustPressed("ui_left_click"))
			{
				GenerateBuilding();
			}
		}
	}

	private void GenerateBuilding()
	{
		Connect(nameof(ShowBuildingSignal), buildingToPlace, "ShowBuilding");
		buildingToPlace.Connect("CanPlaceBuildingSignal", this, nameof(CanPlaceBuilding));

		buildingToPlace.build(this);

		buildingNodes.Add(buildingToPlace);

		ResetBuildingMode();
	}

	private void ResetBuildingMode(bool removeChild = false)
	{
		if (removeChild)
		{
			RemoveChild(buildingToPlace);
		}
		buildingToPlace = null;
		currentMode = ManagementMode.CameraHandlingMode;
	}

	private void CanPlaceBuilding(bool canPlace, int id)
	{
		if (canPlace)
		{
			collidingWithAreaId.Remove(id);
		}
		else
		{
			collidingWithAreaId.Add(id);
		}
		canBuildHere = collidingWithAreaId.Count == 0;
		if (canBuildHere)
		{
			buildingToPlace.ResetColor();
			buildingToPlace.ChangeTransparency(0.75f);
		}
		else
		{
			buildingToPlace.UpdateColorToRed();
		}
	}

	

	public void setResource(ResourceId resourceId, uint quantity)
	{
		Resources.data[(uint)resourceId].quantity = quantity;
		hud.resetResource(resourceId);
	}

	public void addResource(ResourceId resourceId, uint quantity)
	{
		Resources.data[(uint)resourceId].quantity += quantity;
		hud.resetResource(resourceId);
	}

	public void subResource(ResourceId resourceId, uint quantity)
	{
		Resources.data[(uint)resourceId].quantity -= quantity;
		hud.resetResource(resourceId);
	}

	public void addProduced(ResourceId resourceId, int produced)
	{
		Resources.data[(uint)resourceId].produced += produced;
		hud.resetResource(resourceId);
	}

	public void onTimerTimeout()
	{
		for (uint i = 0; i < Resources.data.Length; i++)
		{
			var resource = Resources.data[i];

			if (resource != null && resource.produced != 0)
			{
				if (resource.produced > 0)
				{
					addResource((ResourceId)i, (uint)resource.produced);
				}
				else
				{
					subResource((ResourceId)i, (uint)resource.produced);
				}
			}
		}
	}


	//  // Called every frame. 'delta' is the elapsed time since the previous frame.
	//  public override void _Process(float delta)
	//  {
	//      
	//  }
}
