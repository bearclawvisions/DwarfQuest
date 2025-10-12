using DwarfQuest.Components.Buttons;
using DwarfQuest.Components.UI;
using DwarfQuest.Extensions;

namespace DwarfQuest.Scripts;

public partial class CombatMenu : MenuBase
{
    public FightButton FightButton;
    public TacticButton TacticButton;
    public ItemButton ItemButton;
    public RunButton RunButton;
    
    public override void _Ready()
    {
        this.ClearPlaceholders();
		
        FightButton = new FightButton();
        TacticButton = new TacticButton();
        ItemButton = new ItemButton();
        RunButton = new RunButton();
		
        AddChild(FightButton);
        AddChild(TacticButton);
        AddChild(ItemButton);
        AddChild(RunButton);
        
        Initialize();
    }
    
    public void Reset()
    {
        FightButton.Disabled = false;
        TacticButton.Disabled = false;
        ItemButton.Disabled = false;
        RunButton.Disabled = false;
			
        IsMenuActive = true;
    }
}