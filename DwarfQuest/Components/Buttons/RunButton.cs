using DwarfQuest.Data.Enums;

namespace DwarfQuest.Components.Buttons;

public partial class RunButton : ButtonBase
{
    public RunButton()
    {
        Text = nameof(ButtonType.Run);
    }
}