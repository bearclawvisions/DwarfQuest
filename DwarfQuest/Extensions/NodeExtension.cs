using Godot;

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

        foreach (Node child in node.GetChildren())
        {
            child.QueueFree();
        }
    }
}