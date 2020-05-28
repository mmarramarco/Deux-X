using Godot;
using System;

public class CameraHandler : Camera2D
{
    [Export]
    private readonly float ZoomScaling = 0.1f;
    private Vector2 ZoomScalingVector;
    private Vector2 MinimalZoom = new Vector2(0.3f, 0.3f);
    private Vector2 MaximalZoom = new Vector2(1.5f, 1.5f);
    public MouseState MouseState { get; private set; }

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        ZoomScalingVector = new Vector2(ZoomScaling, ZoomScaling);
        MouseState = MouseState.Released;
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
                        MouseState = MouseState.Clicking;
                        break;
                    default:
                        break;
                }
            }

            if (mouseEvent.IsActionReleased("ui_left_click"))
            {
                MouseState = MouseState.Released;
            }
        }

        if (@event is InputEventMouseMotion mouseMotion)
        {
            if(MouseState == MouseState.Clicking)
            {
                Position -= mouseMotion.Relative * Zoom;
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
