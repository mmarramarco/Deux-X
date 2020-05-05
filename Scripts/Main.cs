using Godot;
using System;

public class Main : TileMap
{
	[Export]
	public int IslandSize = 40;

	private Random random;
	private Camera2D camera;
	private Building buildingToPlace = null;

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		GetAllNodeOnce();
		random = new Random();
		GenerateStartingIsland();
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(float delta)
	{
		if (Input.IsActionJustPressed("ui_select"))
		{
			GenerateStartingIsland();
		}

		if (Input.IsActionJustPressed("key_z"))
		{
			buildingToPlace = Extensions.SmartLoader<CityHall>("res://Scenes/CityHall.tscn");
			AddChild(buildingToPlace);
		}

		if (buildingToPlace != null)
		{
			ProcessMouse();
		}
	}

	private void ProcessMouse()
	{
		var viewport = GetViewport();
		var position = (viewport.GetMousePosition() - viewport.Size / 2) * camera.Zoom + camera.Position;
		var tilePosition = WorldToMap(position);
		var realPosition = MapToWorld(tilePosition) + new Vector2(8f, 8f); // building offset, TODO : make it prettier, and per building.
		buildingToPlace.Position = realPosition;

		if (Input.IsActionJustPressed("ui_right_click"))
		{
			GenerateBuilding(realPosition);
		}
	}

	// TODO : should take a Building in parameters too and should be generalized for every biulding.
	private void GenerateBuilding(Vector2 realPosition)
	{
		buildingToPlace.ChangeTransparency(1);
		buildingToPlace = null;
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
