using DwarfQuest.Data.Enums;

namespace DwarfQuest.Bridge.Components.Buttons;

public partial class TacticButton : ButtonBase
{
    public TacticButton()
    {
        Text = nameof(ButtonType.Tactic);
    }
}