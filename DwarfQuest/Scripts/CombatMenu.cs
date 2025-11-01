using DwarfQuest.Bridge.Extensions;
using DwarfQuest.Components.Buttons;
using DwarfQuest.Components.UI;
using DwarfQuest.Data.Enums;
using Godot;

namespace DwarfQuest.Scripts;

public partial class CombatMenu : MenuBase
{
    public CombatButton FightButton;
    public CombatButton TacticButton;
    public CombatButton ItemButton;
    public CombatButton RunButton;
    
    public override void _Ready()
    {
        this.ClearPlaceholders();
        
        AnchorsPreset = (int)LayoutPreset.BottomLeft;
        Position = new Vector2(Position.X + 10, Position.Y - 10); // padding from screen edges
        Size = new Vector2(50, 0);
		
        FightButton = new CombatButton { Text = nameof(ButtonType.Fight), ActionType = ActionType.Fight };
        TacticButton = new CombatButton { Text = nameof(ButtonType.Tactic), ActionType = ActionType.Tactic };
        ItemButton = new CombatButton { Text = nameof(ButtonType.Item), ActionType = ActionType.Item };
        RunButton = new CombatButton { Text = nameof(ButtonType.Run), ActionType = ActionType.Run };
		
        AddChild(FightButton);
        AddChild(TacticButton);
        AddChild(ItemButton);
        AddChild(RunButton);
        
        Initialize();
    }
    
    public void EnableButtons()
    {
        FightButton.Disabled = false;
        TacticButton.Disabled = false;
        ItemButton.Disabled = false;
        RunButton.Disabled = false;
    }
}