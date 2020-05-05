using System;
using System.Linq;
using System.Threading.Tasks;
using Godot;
using Godot.Collections;

public static class Extensions
{
    /// <summary>
    /// Load a scene smartly. Takes a path to a node to instanciate.
    /// </summary>
    /// <typeparam name="T">The type to return. T must inherit node.</typeparam>
    /// <param name="path">The path to load.</param>
    /// <returns></returns>
    public static T SmartLoader<T>(string path) where T : Node
    {
        try
        {
            var resource = ResourceLoader.Load(path);
            var packedScene = (PackedScene)resource;
            var instance = (T)packedScene.Instance();
            return instance;
        }
        catch (Exception exception)
        {
            GD.Print($"Exception while smart loading a scene : {exception.Message}");
            throw exception;
        }
    }

    /// <summary>
    /// A background loader.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="path"></param>
    /// <returns></returns>
    public static async Task<T> BackGroundLoader<T>(string path) where T : Node
    {
        return await Task.Run(() => SmartLoader<T>(path)).ConfigureAwait(false);
    }

    /// <summary>
    /// Search for children of Type T and return them in a new Array.
    /// </summary>
    /// <typeparam name="T">The type to return. T must inherit node.</typeparam>
    /// <param name="node">The node from which we will start searching.</param>
    /// <returns></returns>
    public static Array<T> FindChildrenOfType<T>(this Node node) where T : Node
    {
        var children = node.GetChildren();
        var nodes = new Array<Node>(children);
        var uncasted = nodes.Where(_ => _.GetType() == typeof(T));

        Array<T> result = new Array<T>();
        foreach (var toCast in uncasted)
        {
            result.Add((T)toCast);
        }

        return result;
    }
}
