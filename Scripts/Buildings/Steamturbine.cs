using Godot;
using System;
using DeuxX.Scripts;

public class Steamturbine : BuildingNode
{
    // Declare member variables here. Examples:
    // private int a = 2;
    // private string b = "text";

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        
    }

    public override void build(Game game)
    {
        base.build(game);

        game.subResource(ResourceId.Workers, 2);
        game.addProduced(ResourceId.Water, -3);
        game.addProduced(ResourceId.Electricity, 5);
    }

    //  // Called every frame. 'delta' is the elapsed time since the previous frame.
    //  public override void _Process(float delta)
    //  {
    //      
    //  }
}
