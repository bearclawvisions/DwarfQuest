using DwarfQuest.Data.Enums;

namespace DwarfQuest.Bridge.Components.Buttons;

public partial class RunButton : ButtonBase
{
    public RunButton()
    {
        Text = nameof(ButtonType.Run);
    }
}