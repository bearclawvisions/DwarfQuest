using Godot;
using System;

namespace DwarfQuest.Extensions;

public static class NodeExtension
{
    /// <summary>
    /// Removes all child nodes of this node at runtime.
    /// Skips doing so in the editor.
    /// </summary>
    public static void ClearPlaceholders(this Node node, bool includeEditor = false)
    {
        if (Engine.IsEditorHint() && !includeEditor)
            return;

        foreach (var child in node.GetChildren())
        {
            child.QueueFree();
        }
    }

    /// <summary>
    /// Add a timer to enable a short wait period for something else to finish.
    /// Like waiting for a tween effect to finish. Default is 1-second wait.
    /// </summary>
    public static void AddWaitTimer(this Node node, Action methodCall, int seconds = 1)
    {
        var secondsDouble = (double)seconds;
        var timer = new Timer { WaitTime = secondsDouble, OneShot = true };
        node.AddChild(timer);
        timer.Start();
        timer.Timeout += methodCall;
    }
}