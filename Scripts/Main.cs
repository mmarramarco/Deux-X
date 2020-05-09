using Godot;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

public class Main : TileMap
{
	[Export]
	public int IslandSize = 40;

	public List<Building> Buildings = new List<Building>();

	private Random random;
	private Camera2D camera;
	private Building buildingToPlace = null;
	private bool canBuildHere = true;
	private List<int> collidingWithAreaId = new List<int>();
	private ManagementMode currentMode = ManagementMode.CameraHandlingMode;
	private HUD hud;

	[Signal]
	private delegate void ShowBuildingSignal(bool show);

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		GetAllNodeOnce();
		random = new Random();
		GenerateStartingIsland();
		hud = GetNode<HUD>("CanvasLayer/HUD");
		hud.Connect("SwitchToUpgradeModeSignal", this, nameof(SwitchToUpgradeMode));
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(float delta)
	{
		if (Input.IsActionJustPressed("ui_select"))
		{
			GenerateStartingIsland();
		}

		if (Input.IsActionJustPressed("tab_key"))
		{
			EmitSignal(nameof(ShowBuildingSignal), false);
		}
		if (Input.IsActionJustReleased("tab_key"))
		{
			EmitSignal(nameof(ShowBuildingSignal), true);
		}

		if (Input.IsActionJustPressed("key_z") && buildingToPlace == null)
		{
			buildingToPlace = Extensions.SmartSceneLoader<CityHall>("res://Scenes/CityHall.tscn");
			buildingToPlace.Initialize();
			AddChild(buildingToPlace);
			currentMode = ManagementMode.BuildingMode; 
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

	private void SwitchToUpgradeMode(bool toggle)
	{
		GD.Print($"Switched to upgrade mode : {toggle}");
		foreach(var building in Buildings)
		{
			building.SwitchToUpgradeMode(toggle);
		}
	}

	private void BuildingMode()
	{
		var viewport = GetViewport();
		var position = (viewport.GetMousePosition() - viewport.Size / 2) * camera.Zoom + camera.Position;
		var tilePosition = WorldToMap(position);
		var realPosition = MapToWorld(tilePosition) + new Vector2(8f, 8f); // building offset, TODO : make it prettier, and per building.
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
		buildingToPlace.ChangeTransparency(1);
		Buildings.Add(buildingToPlace);
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
			buildingToPlace.UpdateColorToRed();
		}
		else
		{
			buildingToPlace.ResetColor();
		}
	}

	private void GenerateStartingIsland()
	{
		Clear();
		var startringPoint = new Vector2(0, 0);
		GenerateIsland(startringPoint);
	}

	private void GenerateIsland(Vector2 newIslandCenter)
	{
		var tileMapCenter = WorldToMap(newIslandCenter);
		var startingPoint = new Vector2(tileMapCenter.x - IslandSize / 2, tileMapCenter.y - IslandSize / 2);
		Vector2 newCellPosition;
		for(int i = 0; i < IslandSize; i++)
		{
			for (int j = 0; j < IslandSize; j++)
			{
				newCellPosition = new Vector2(startingPoint.x + i, startingPoint.y + j);
				GenerateCell(newCellPosition);
			}
		}
	}

	private void GenerateCell(Vector2 newCellPosition)
	{
		var cell = random.Next(0, 2);
		SetCellv(newCellPosition, cell);
		UpdateBitmaskArea(newCellPosition);
	}

	private void GetAllNodeOnce()
	{
		camera = GetNode<Camera2D>("Camera2D");
	}

}
