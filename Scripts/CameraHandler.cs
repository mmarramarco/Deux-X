using Godot;
using System;

public class CameraHandler : Camera2D
{
    [Export]
    private float ZoomScaling = 0.1f;
    private Vector2 ZoomScalingVector;
    private Vector2 MinimalZoom = new Vector2(0.5f, 0.5f);
    private Vector2 MaximalZoom = new Vector2(1.5f, 1.5f);
    private MouseState currentState = MouseState.Released;

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        ZoomScalingVector = new Vector2(ZoomScaling, ZoomScaling);
    }

    public override void _UnhandledInput(InputEvent @event)
    {
        if (@event is InputEventMouseButton mouseEvent)
        {
            if (mouseEvent.IsPressed())
            {
                switch ((ButtonList)mouseEvent.ButtonIndex)
                {
                    case ButtonList.WheelUp:
                        CameraZoom();
                        break;
                    case ButtonList.WheelDown:
                        CameraUnzoom();
                        break;
                    case ButtonList.Left:
                        currentState = MouseState.Clicking;
                        GD.Print("Clicking !");
                        break;
                    default:
                        break;
                }
            }

            if (mouseEvent.IsActionReleased("ui_left_click"))
            {
                currentState = MouseState.Released;
                GD.Print("Releasing");
            }
        }

        if (@event is InputEventMouseMotion mouseMotion)
        {
            if(currentState == MouseState.Clicking)
            {
                GD.Print("Dragging.");
                Position -= mouseMotion.Relative;
            }
        }
    }

    private void CameraZoom()
    {
        if(Zoom > MinimalZoom)
        {
            Zoom -= ZoomScalingVector;
        }
    }

    private void CameraUnzoom()
    {
        if (Zoom < MaximalZoom)
        {
            Zoom += ZoomScalingVector;
        }
    }
}
