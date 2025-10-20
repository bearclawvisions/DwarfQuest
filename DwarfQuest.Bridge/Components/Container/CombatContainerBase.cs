using DwarfQuest.Bridge.Components.Character;
using DwarfQuest.Data.Enums;
using Godot;

namespace DwarfQuest.Bridge.Components.Container;

public partial class CombatContainerBase : Node2D
{
    private int _index = 0;
    private bool _canSelect;
	
    public List<CharacterBase> Participants;
	
    public bool CanSelect
    {
        get => _canSelect;
        set => SetMenuActive(value);
    }
	
    public override void _UnhandledInput(InputEvent @event)
    {
        if (@event is InputEventKey { Pressed: true, Keycode: Key.W } && CanSelect)
            MoveFocus((int)MenuDirection.Up);
        
        if (@event is InputEventKey { Pressed: true, Keycode: Key.S } && CanSelect)
            MoveFocus((int)MenuDirection.Down);
    }
    
    public void Reset()
    {
        SetMenuActive(false);
    }
    
    private void MoveFocus(int direction)
    {
        Participants[_index].Deselect();
		
        _index = (_index + direction + Participants.Count) % Participants.Count;
        Participants[_index].Select();
    }
	
    private void SetMenuActive(bool isMenuActive)
    {
        _canSelect = isMenuActive;

        if (isMenuActive)
        {
            Participants[_index].Select();
        }
        else
        {
            Participants[_index].Deselect();
            _index = 0;
        }
    }
}