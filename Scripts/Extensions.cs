using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
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
    public static T SmartSceneLoader<T>(string path) where T : Node
    {
        try
        {
            var packedScene = ResourceLoader.Load<PackedScene>(path);
            var instance = packedScene.Instance() as T;
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
        return await Task.Run(() => SmartSceneLoader<T>(path)).ConfigureAwait(false);
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

    /// <summary>
    /// Not sure if it works. Awaits for a node child of <paramref name="parent"/> to be ready, then execute the <paramref name="action"/> method.
    /// </summary>
    /// <param name="parent">The parent node.</param>
    /// <param name="nodeToAwait">The node to await.</param>
    /// <param name="action">The action to execute once <paramref name="nodeToAwait"/>is ready.</param>
    public static void AwaitForNodeToBeReady(this Node parent, Node nodeToAwait, Action action)
    {
        var result = Task.Run(() => AwaitForNodeToBeReadyAsync(parent, nodeToAwait, action));
    }

    private static async Task AwaitForNodeToBeReadyAsync(this Node parent, Node nodeToAwait, Action action)
    {
        GD.Print($"Awaiting for node {nodeToAwait.Name} to be ready, child of {parent.Name}.");
        await parent.ToSignal(nodeToAwait, "ready");
        action.Invoke();
    } 
}
