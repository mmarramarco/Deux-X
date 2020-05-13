using Godot;
using System;

/// <summary>
/// The building class. Defines behaviour for all buildings.
/// </summary>
public abstract class BuildingNode : Area2D
{
	[Export]
	private readonly int MaxLevel = 2;

	private string name;
	private int level = 1;
	//private Recipe recipeForNextLevel;
	private Sprite sprite;
	private Color defaultColor;
	private bool upgradeMode = false;

    public Vector2 Offset { get; set; }

    [Signal]
	private delegate void CanPlaceBuildingSignal(bool canPlace, int id);

	public void Initialize()
	{
		ZIndex = 10;
		Connect("area_entered", this, nameof(OnAreaEntered));
		Connect("area_exited", this, nameof(OnAreaExited));
		Connect("input_event", this, nameof(OnInputEven));
		sprite = GetNode<Sprite>("Sprite");
		defaultColor = sprite.Modulate;
		ChangeTransparency(0.75f);
	}

	public void OnInputEven(Viewport viewport, InputEvent @event, int shapeIndex)
	{
		if (@event is InputEventMouseButton mouseEvent)
		{
			if (mouseEvent.IsPressed())
			{
				if ((ButtonList)mouseEvent.ButtonIndex == ButtonList.Left && 
					CanUpgrade() &&
					upgradeMode)
				{
					Upgrade();
				}
			}
		}
	}

	private void Upgrade()
	{
		sprite.Frame += 2;
		level++;

		// we just reset the color of the current upgradable building.
		SwitchToUpgradeMode(true);
	}

	private void OnAreaEntered(Area2D area)
	{
		if(area is BuildingNode)
		{
			EmitSignal(nameof(CanPlaceBuildingSignal), false, GetInstanceId());
		}
	}

	private void OnAreaExited(Area2D area)
	{
		if (area is BuildingNode)
		{
			EmitSignal(nameof(CanPlaceBuildingSignal), true, GetInstanceId());
		}
	}

	public void ShowBuilding(bool show)
	{
		var frame = show ? -1 : 1;
		sprite.Frame += frame;
	}

	public bool CanUpgrade()
	{
		return level < MaxLevel;
	}

	internal void SwitchToUpgradeMode(bool toggle)
	{
		upgradeMode = toggle;
		ResetColor();
		if (upgradeMode)
		{
			if (CanUpgrade())
			{
				UpdateColorToGreen();
			}
			else
			{
				UpdateColorToRed();
			}
		}
	}

	public override string ToString()
	{
		return name;
	}

	public void ChangeTransparency(float alpha)
	{
		sprite.Modulate = new Color(sprite.Modulate.r, sprite.Modulate.g, sprite.Modulate.b, alpha);
	}

	public void UpdateColorToRed()
	{
		sprite.Modulate = new Color(1.5f, sprite.Modulate.g, sprite.Modulate.b, sprite.Modulate.a);
	}

	public void UpdateColorToGreen()
	{
		sprite.Modulate = new Color(sprite.Modulate.r, 1.5f, sprite.Modulate.b, sprite.Modulate.a);
	}

	public void ResetColor()
	{
		sprite.Modulate = defaultColor;
	}
}
