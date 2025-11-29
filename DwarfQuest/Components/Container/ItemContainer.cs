using DwarfQuest.Data.Enums;
using DwarfQuest.Data.Models;
using DwarfQuest.Scripts;
using Godot;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DwarfQuest.Components.Container;

public partial class ItemContainer : VBoxContainer
{
    private const float ContainerWidth = 250f; // ItemEntry inherits this
    private const float ContainerHeight = 50f; // ItemEntry inherits this
    
    public async Task Initialize(List<Item> items)
    {
        SetBase();
        
        foreach (var item in items)
        {
            var entry = new ItemEntry();
            entry.Initialize(item);
            
            entry.Modulate = new Color(1, 1, 1, 0);
            AddChild(entry);
		
            var tween = CreateTween();
            tween.TweenProperty(entry, GodotProperty.ModulateAlpha, 1.0, 0.2);
		
            // item appearance animation
            await ToSignal(GetTree().CreateTimer(0.2), SceneTreeTimer.SignalName.Timeout);
        }
    }

    private void SetBase()
    {
        var windowSize = AutoLoader.GetWindowSize();
		var horizontalWidth = windowSize.X * 0.25f; // 25% of the screen width
        var verticalHeight = windowSize.Y * 0.42f; // 42% of the screen height
        
        var leftLocation = horizontalWidth - ContainerWidth / 2;
        
        Size = new Vector2(ContainerWidth, ContainerHeight);
        Position = new Vector2(leftLocation, verticalHeight);
    }
}