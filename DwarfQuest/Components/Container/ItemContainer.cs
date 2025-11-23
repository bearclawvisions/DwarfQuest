using DwarfQuest.Data.Enums;
using DwarfQuest.Data.Models;
using Godot;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DwarfQuest.Components.Container;

public partial class ItemContainer : VBoxContainer
{
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
		
            await ToSignal(GetTree().CreateTimer(0.2), SceneTreeTimer.SignalName.Timeout);
        }
    }

    private void SetBase()
    {
        Size = new Vector2(250f, 50f);
        Position = new Vector2(63f, 150f);
    }
}