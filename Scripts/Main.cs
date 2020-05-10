using DeuxX.Scripts;
using Godot;
using System;
using System.Collections.Generic;

public class Main : TileMap
{
	[Export]
	public int IslandSize = 40;

	public List<BuildingNode> Buildings = new List<BuildingNode>();

	private Random random;
	private Camera2D camera;
	private BuildingNode buildingToPlace = null;
	private bool canBuildHere = true;
	private List<int> collidingWithAreaId = new List<int>();
	private ManagementMode currentMode = ManagementMode.CameraHandlingMode;
	private HUD hud;
	private Buildings buildingsDataContainer = new Buildings();

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
		hud.Initialize(this);
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
		buildingToPlace = buildingsDataContainer.GetBuildingNode(buildingId);
		buildingToPlace.Initialize();
		AddChild(buildingToPlace);
		currentMode = ManagementMode.BuildingMode;
	}

	private void SwitchToUpgradeMode(bool toggle)
	{
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
			buildingToPlace.ResetColor();
			buildingToPlace.ChangeTransparency(0.75f);
		}
		else
		{
			buildingToPlace.UpdateColorToRed();
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
