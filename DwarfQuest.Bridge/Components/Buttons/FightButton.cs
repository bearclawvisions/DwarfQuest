using DwarfQuest.Data.Enums;

namespace DwarfQuest.Bridge.Components.Buttons;

public partial class FightButton : ButtonBase
{
    public FightButton()
    {
        Text = nameof(ButtonType.Fight);
    }
}