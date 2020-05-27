using Godot;
using System;
using System.Collections.Generic;
using DeuxX.Scripts;

public class CityHall : BuildingNode
{
	public List<BuildingNode> Buildings;

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		
	}

	public override void build(Main main)
    {
		base.build(main);

		main.addResource(ResourceId.Workers, 10);
		main.addProduced(ResourceId.Electricity, 5);
    }

	//  // Called every frame. 'delta' is the elapsed time since the previous frame.
	//  public override void _Process(float delta)
	//  {
	//      
	//  }
}
