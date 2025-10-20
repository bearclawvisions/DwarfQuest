using DwarfQuest.Bridge.Managers;
using Godot;

namespace DwarfQuest.Scripts;

public partial class AutoLoader : Node
{
    public static AutoLoader Instance { get; private set; }
	private static Vector2 _viewportSize;

    public override void _Ready()
    {
        Instance = this;
        GameManager.Initialize();
        ResourceManager.Initialize();
        
		_viewportSize = GetViewport().GetVisibleRect().Size;
    }

    public static Vector2 GetWindowSize()
    {
        return _viewportSize;
    }
}