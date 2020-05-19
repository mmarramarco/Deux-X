using Godot;
using System;
using System.Reflection.Emit;
using DeuxX.Scripts;

public class ResourceNode : Control
{
    // Declare member variables here. Examples:
    // private int a = 2;
    // private string b = "text";

    // Called when the node enters the scene tree for the first time.

    uint resourceId;

    public void init(ResourceId resourceId)
    {
        this.resourceId = (uint)resourceId;
    }

    public void reset()
    {
        var s = Resources.data[resourceId].getValues();

        var node = GetNode<Godot.Label>("Quantity");

        node.Text = s[0];

        node = GetNode<Godot.Label>("Produced");

        node.Text = s[1];
    }

    public override void _Ready()
    {
        reset();
    }

    

    //  // Called every frame. 'delta' is the elapsed time since the previous frame.
    //  public override void _Process(float delta)
    //  {
    //      
    //  }
}
