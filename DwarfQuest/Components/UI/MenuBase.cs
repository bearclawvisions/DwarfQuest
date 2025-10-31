using DwarfQuest.Data.Enums;
using Godot;
using System.Collections.Generic;
using DwarfQuest.Components.Buttons;

namespace DwarfQuest.Components.UI;

public partial class MenuBase : VBoxContainer
{
    private readonly List<CombatButton> _buttons = [];
    private int _index = 0;
    private bool _isMenuActive;
    
    public bool IsMenuActive 
    { 
        get => _isMenuActive;
        set => SetMenuActive(value);
    }

    protected void Initialize()
    {
        var children = GetChildren();

        foreach (var child in children)
        {
            if (child is not CombatButton button) continue;
            _buttons.Add(button);
        }

        if (_buttons.Count > 0)
            _buttons[_index].GrabFocus();
    }

    public override void _UnhandledInput(InputEvent @event)
    {
        if (!IsMenuActive) return;
        
        if (@event is InputEventKey { Pressed: true, Keycode: Key.W } && IsMenuActive)
            MoveFocus((int)MenuDirection.Up);
        
        if (@event is InputEventKey { Pressed: true, Keycode: Key.S } && IsMenuActive)
            MoveFocus((int)MenuDirection.Down);
    }
    
    private void MoveFocus(int direction)
    {
        _index = (_index + direction + _buttons.Count) % _buttons.Count;
        _buttons[_index].GrabFocus();
    }
    
    private void SetMenuActive(bool isMenuActive)
    {
        _isMenuActive = isMenuActive;
        
        if (!isMenuActive)
            _index = 0;
    }
}