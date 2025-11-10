using DwarfQuest.Components.Buttons;
using DwarfQuest.Components.UI;
using DwarfQuest.Data.Enums;
using Godot;

namespace DwarfQuest.Scripts;

public partial class RunMenu : MenuBase
{
    public CombatButton Yes;
    public CombatButton No;
    
    public override void _Ready()
    {
        AnchorsPreset = (int)LayoutPreset.BottomLeft;
        Position = new Vector2(Position.X + 50, Position.Y - 10); // padding from screen edges
        Size = new Vector2(50, 0);
		
        Yes = new CombatButton { Text = nameof(ButtonType.Yes) };
        No = new CombatButton { Text = nameof(ButtonType.No) };
		
        AddChild(Yes);
        AddChild(No);
        
        Initialize();
    }
    
    public void EnableButtons()
    {
        Yes.Disabled = false;
        No.Disabled = false;
    }
}