using Godot;
using System;
using System.Collections.Generic;
using DeuxX.Scripts;

public class Extractor : BuildingNode
{
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		
	}

	public override void build(Main main)
    {
		base.build(main);

		main.subResource(ResourceId.Workers, 2);
		main.addProduced(ResourceId.Electricity, -3);
		main.addProduced(ResourceId.Water, 5);
    }

	//  // Called every frame. 'delta' is the elapsed time since the previous frame.
	//  public override void _Process(float delta)
	//  {
	//      
	//  }
}
