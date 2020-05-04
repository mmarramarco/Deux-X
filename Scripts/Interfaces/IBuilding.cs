using Godot;
using System;

public interface IBuilding
{
    string Name { get; set; }

    int Level { get; set; }

    // TODO : Add recipe class.
    // Recipe RecipeForNextLevel { get; set; }

    bool CanEvolve();
}
