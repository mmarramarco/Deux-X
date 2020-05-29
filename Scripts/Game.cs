using Godot;
using System;
using DeuxX.Scripts;
using System.Collections.Generic;
using System.Linq;

public class Game : Node
{
    [Signal]
    private delegate void ShowBuildingSignal(bool show);

    private Buildings buildings = new Buildings();
    private Resources resources = new Resources();

    public List<BuildingNode> buildingNodes = new List<BuildingNode>();

    private Camera2D camera;
    private List<BuildingNode> buildingsToPlace = new List<BuildingNode>();
    private bool canBuildHere = true;
    private List<int> collidingWithAreaId = new List<int>();
    private ManagementMode currentMode = ManagementMode.CameraHandlingMode;
    private HUD hud;

	private World world;

	private YSort ysort;

	public override void _Ready()
    {
		camera = GetNode<Camera2D>("Camera2D");
		world = GetNode<World>("World");
		hud = GetNode<HUD>("CanvasLayer/HUD");
		ysort = GetNode<YSort>("YSort");

		world.GenerateStartingIsland();
        
        hud.Connect("SwitchToUpgradeModeSignal", this, nameof(SwitchToUpgradeMode));
        hud.Initialize(this);
    }

	public override void _Process(float delta)
	{
		if (Input.IsActionJustPressed("ui_select"))
		{
			world.GenerateStartingIsland();
		}

		if (Input.IsActionJustPressed("tab_key"))
		{
			EmitSignal(nameof(ShowBuildingSignal), false);
		}
		if (Input.IsActionJustReleased("tab_key"))
		{
			EmitSignal(nameof(ShowBuildingSignal), true);
		}

		switch (currentMode)
		{
			case ManagementMode.BuildingMode:
				BuildingMode();
				break;

			case ManagementMode.BuildingTunnel0:
				BuildingTunnel0();
				break;

			case ManagementMode.BuildingTunnel1:
				BuildingTunnel1();
				break;

			default:
				break;
		}

		if (Input.IsActionJustPressed("ui_right_click"))
		{
			ResetBuildingMode(removeChild: true);
		}
	}

	private void StartBuilding(BuildingId buildingId)
	{
		var buildingToPlace = Buildings.data[(uint)buildingId].scene.Instance() as BuildingNode;
		
		buildingToPlace.Initialize();
		ysort.AddChild(buildingToPlace);

		buildingsToPlace.Add(buildingToPlace);

		switch (buildingId)
        {
			case BuildingId.Tunnel:
				currentMode = ManagementMode.BuildingTunnel0;
				break;

			default:
				currentMode = ManagementMode.BuildingMode;
				break;
		}
	}

	private void SwitchToUpgradeMode(bool toggle)
	{
		foreach (var building in buildingNodes)
		{
			building.SwitchToUpgradeMode(toggle);
		}
	}

	private void buildingFollowCursor(BuildingNode building)
    {
		var viewport = GetViewport();
		var position = (viewport.GetMousePosition() - viewport.Size / 2) * camera.Zoom + camera.Position;
		var tilePosition = world.WorldToMap(position);

		var realPosition = world.MapToWorld(tilePosition) + building.Offset; // building offset, TODO : make it prettier, and per building.
		building.Position = realPosition;
	}

	private void BuildingMode()
	{
		var buildingToPlace = buildingsToPlace.Last();

		buildingFollowCursor(buildingToPlace);

		if (!buildingToPlace.isInContact())
		{
			if (Input.IsActionJustPressed("ui_left_click"))
			{
				GenerateBuilding();
			}
		}
	}

	private void BuildingTunnel0()
	{
		var buildingToPlace = buildingsToPlace.Last();

		buildingFollowCursor(buildingToPlace);

		if (!buildingToPlace.isInContact())
		{
			if (Input.IsActionJustPressed("ui_left_click"))
			{
				var buildingToPlace1 = Buildings.data[(uint)BuildingId.Tunnel].scene.Instance() as BuildingNode;

				buildingToPlace1.Initialize();
				ysort.AddChild(buildingToPlace1);

				buildingsToPlace.Add(buildingToPlace1);

				currentMode = ManagementMode.BuildingTunnel1;
			}
		}
	}

	private void BuildingTunnel1()
	{
		var buildingToPlaceFirst = buildingsToPlace.First();
		var buildingToPlaceLast = buildingsToPlace.Last();

		buildingFollowCursor(buildingToPlaceLast);

		//version crade a opti
		//reutiliser les tunnels deja construit plutot que de les delete/new a chaque frame
		if (buildingsToPlace.Count > 2)
		{
			foreach (var buildingNode in buildingsToPlace.GetRange(1, buildingsToPlace.Count - 2))
			{
				ysort.RemoveChild(buildingNode);
			}

			buildingsToPlace.RemoveRange(1, buildingsToPlace.Count - 2);
		}

		if (buildingToPlaceFirst.Position.DistanceTo(buildingToPlaceLast.Position) > 16)
        {
			int posXFirst = 0;
			int posXLast = 0;
			bool posXb = false;

			if(buildingToPlaceFirst.Position.x < buildingToPlaceLast.Position.x)
            {
				posXFirst = (int)buildingToPlaceFirst.Position.x + 16;
				posXLast = (int)buildingToPlaceLast.Position.x;
				posXb = true;

			}
            else if(buildingToPlaceFirst.Position.x > buildingToPlaceLast.Position.x)
			{
				posXFirst = (int)buildingToPlaceLast.Position.x + 16;
				posXLast = (int)buildingToPlaceFirst.Position.x;
				posXb = true;
			}

            if (posXb)
            {
				for (int i = posXFirst; i < posXLast; i += 16)
				{
					var buildingToPlace1 = Buildings.data[(uint)BuildingId.Tunnel].scene.Instance() as BuildingNode;

					buildingToPlace1.Initialize();
					ysort.AddChild(buildingToPlace1);

					buildingsToPlace.Insert(buildingsToPlace.Count - 1, buildingToPlace1);

					buildingToPlace1.Position = new Vector2(i, buildingToPlaceFirst.Position.y);
				}
			}

			int posYFirst = 0;
			int posYLast = 0;
			bool posYb = false;

			if(buildingToPlaceFirst.Position.y < buildingToPlaceLast.Position.y)
            {
				posYFirst = (int)buildingToPlaceFirst.Position.y + 16;
				posYLast = (int)buildingToPlaceLast.Position.y;
				posYb = true;
			}
			else if(buildingToPlaceFirst.Position.y > buildingToPlaceLast.Position.y)
            {
				posYFirst = (int)buildingToPlaceLast.Position.y + 16;
				posYLast = (int)buildingToPlaceFirst.Position.y;
				posYb = true;
			}

            if (posYb)
            {
				for (int i = posYFirst; i < posYLast; i += 16)
				{
					var buildingToPlace1 = Buildings.data[(uint)BuildingId.Tunnel].scene.Instance() as BuildingNode;

					buildingToPlace1.Initialize();
					ysort.AddChild(buildingToPlace1);

					buildingsToPlace.Insert(buildingsToPlace.Count - 1, buildingToPlace1);

					buildingToPlace1.Position = new Vector2(buildingToPlaceLast.Position.x, i);
				}
			}

			if(posXb && posYb){
				var buildingToPlace1 = Buildings.data[(uint)BuildingId.Tunnel].scene.Instance() as BuildingNode;

				buildingToPlace1.Initialize();
				ysort.AddChild(buildingToPlace1);

				buildingsToPlace.Insert(buildingsToPlace.Count - 1, buildingToPlace1);

				buildingToPlace1.Position = new Vector2(buildingToPlaceLast.Position.x, buildingToPlaceFirst.Position.y);
			}
		}

		if (!buildingToPlaceLast.isInContact())
		{
			if (Input.IsActionJustPressed("ui_left_click"))
			{
				GD.Print(buildingsToPlace.Count);
			}
		}
	}


	private void GenerateBuilding()
	{
		var buildingToPlace = buildingsToPlace.Last();

		Connect(nameof(ShowBuildingSignal), buildingToPlace, "ShowBuilding");

		buildingToPlace.build(this);

		buildingNodes.Add(buildingToPlace);

		ResetBuildingMode();
	}

	private void ResetBuildingMode(bool removeChild = false)
	{
		if (removeChild)
		{
			foreach(var buildingToPlace in buildingsToPlace)
            {
				ysort.RemoveChild(buildingToPlace);
			}
		}

		buildingsToPlace.Clear();

		currentMode = ManagementMode.CameraHandlingMode;
	}

	public void setResource(ResourceId resourceId, uint quantity)
	{
		Resources.data[(uint)resourceId].quantity = quantity;
		hud.resetResource(resourceId);
	}

	public void addResource(ResourceId resourceId, uint quantity)
	{
		Resources.data[(uint)resourceId].quantity += quantity;
		hud.resetResource(resourceId);
	}

	public void subResource(ResourceId resourceId, uint quantity)
	{
		Resources.data[(uint)resourceId].quantity -= quantity;
		hud.resetResource(resourceId);
	}

	public void addProduced(ResourceId resourceId, int produced)
	{
		Resources.data[(uint)resourceId].produced += produced;
		hud.resetResource(resourceId);
	}

	public void onTimerTimeout()
	{
		for (uint i = 0; i < Resources.data.Length; i++)
		{
			var resource = Resources.data[i];

			if (resource != null && resource.produced != 0)
			{
				if (resource.produced > 0)
				{
					addResource((ResourceId)i, (uint)resource.produced);
				}
				else
				{
					subResource((ResourceId)i, (uint)resource.produced);
				}
			}
		}
	}


	//  // Called every frame. 'delta' is the elapsed time since the previous frame.
	//  public override void _Process(float delta)
	//  {
	//      
	//  }
}
