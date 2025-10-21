using DwarfQuest.Data.Enums;

namespace DwarfQuest.Components.Buttons;

public partial class RunButton : ButtonBase
{
    public ActionType ActionType = ActionType.Run;
    
    public RunButton()
    {
        Text = nameof(ButtonType.Run);
    }
}