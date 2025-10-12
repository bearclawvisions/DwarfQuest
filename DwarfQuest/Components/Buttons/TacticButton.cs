using DwarfQuest.Data.Enums;

namespace DwarfQuest.Components.Buttons;

public partial class TacticButton : ButtonBase
{
    public TacticButton()
    {
        Text = nameof(ButtonType.Tactic);
    }
}