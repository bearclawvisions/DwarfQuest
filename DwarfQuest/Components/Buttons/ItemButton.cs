using DwarfQuest.Data.Enums;

namespace DwarfQuest.Components.Buttons;

public partial class ItemButton : ButtonBase
{
    public ItemButton()
    {
        Text = nameof(ButtonType.Item);
    }
}