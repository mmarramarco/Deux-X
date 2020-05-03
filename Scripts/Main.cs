using Godot;
using System;

public class Main : TileMap
{
	[Export]
	public int IslandSize = 40;

	private Random random;

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		GetAllNodeOnce();
		random = new Random();
		GenerateStartingIsland();
	}

	private void GenerateStartingIsland()
	{
		Clear();
		var screenCenter = GetViewport().Size / 2;
		GenerateIsland(screenCenter);
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
		if(cell == 1)
		{
			SetCellv(newCellPosition, 0);
			UpdateBitmaskArea(newCellPosition);
		}
	}

	private void GetAllNodeOnce()
	{
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(float delta)
	{
		if (Input.IsActionJustPressed("ui_select"))
		{
			GenerateStartingIsland();
		}
	}
}
