using Godot;
using System;

/// <summary>
/// The building class. Defines behaviour for all buildings.
/// </summary>
public abstract class Building : Area2D
{
    private string name;
    private int level = 1;
    //private Recipe recipeForNextLevel;

    public bool CanEvolve()
    {
        return true;
    }

    public override string ToString()
    {
        return name;
    }

    public void ChangeTransparency(float alpha)
    {
        var s = GetNode<Sprite>("Sprite2D");
        GD.Print($"sprite in ChangeTransparency : {s.GetPath()}");
        s.Modulate = new Color(s.Modulate.r, s.Modulate.g, s.Modulate.b, alpha); 
    }
}
