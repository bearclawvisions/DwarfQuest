using DwarfQuest.Data.Enums;

namespace DwarfQuest.Components.Buttons;

public partial class FightButton : ButtonBase
{
    public FightButton()
    {
        Text = nameof(ButtonType.Fight);
    }
}