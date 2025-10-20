using DwarfQuest.Data.Enums;

namespace DwarfQuest.Bridge.Components.Buttons;

public partial class ItemButton : ButtonBase
{
    public ItemButton()
    {
        Text = nameof(ButtonType.Item);
    }
}