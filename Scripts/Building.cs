using Godot;
using System;

/// <summary>
/// The building class. Defines behaviour for all buildings.
/// </summary>
public abstract class Building : Area2D
{
    [Export]
    private readonly int MaxLevel = 2;

    private string name;
    private int level = 1;
    //private Recipe recipeForNextLevel;
    private Sprite sprite;
    private Color previousColor;

    [Signal]
    private delegate void CanPlaceBuildingSignal(bool canPlace, int id);

    public void Initialize()
    {
        Connect("area_entered", this, nameof(OnAreaEntered));
        Connect("area_exited", this, nameof(OnAreaExited));
        Connect("input_event", this, nameof(OnInputEven));
        sprite = GetNode<Sprite>("Sprite2D");
        previousColor = sprite.Modulate;
    }

    public void OnInputEven(Viewport viewport, InputEvent @event, int shapeIndex)
    {
        if (@event is InputEventMouseButton mouseEvent)
        {
            if (mouseEvent.IsPressed())
            {
                if ((ButtonList)mouseEvent.ButtonIndex == ButtonList.Left)
                {
                    if (CanEvolve() && (level < MaxLevel))
                    {
                        sprite.Frame += 2;
                    }
                }
            }
        }
    }

    private void OnAreaEntered(Area2D area)
    {
        if(area is Building)
        {
            EmitSignal(nameof(CanPlaceBuildingSignal), false, GetInstanceId());
        }
    }

    private void OnAreaExited(Area2D area)
    {
        if (area is Building)
        {
            EmitSignal(nameof(CanPlaceBuildingSignal), true, GetInstanceId());
        }
    }

    public void ShowBuilding(bool show)
    {
        var frame = show ? 0 : 1;
        sprite.Frame = frame;
    }

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
        sprite.Modulate = new Color(sprite.Modulate.r, sprite.Modulate.g, sprite.Modulate.b, alpha);
    }

    internal void UpdateColor(bool canPlace)
    {
        if (canPlace)
        {
            sprite.Modulate = previousColor;
        }
        else
        {
            sprite.Modulate = new Color(2.0f, sprite.Modulate.g, sprite.Modulate.b, sprite.Modulate.a);
        }
    }
}
