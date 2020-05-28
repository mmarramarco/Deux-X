using DeuxX.Scripts;
using Godot;
using System;
using System.Collections.Generic;

public class World : TileMap
{
	[Export]
	public int IslandSize = 40;

	private Random random;


	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		random = new Random();
	}

	public void GenerateStartingIsland()
	{
		Clear();

		var noise = new OpenSimplexNoise();

		noise.Seed = (int)GD.Randi();
		noise.Octaves = 4;
		noise.Period = 20;
		noise.Persistence = 0.8f;

		for (int i = 0; i < IslandSize; i++)
		{
			for (int j = 0; j < IslandSize; j++)
			{
				var pos = new Vector2(i, j);

				SetCellv(pos, (int)Mathf.Lerp(0, 2, (noise.GetNoise2dv(pos) + 1) / 2));
				UpdateBitmaskArea(pos);
			}
		}
	}
}
