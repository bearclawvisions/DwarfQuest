using DwarfQuest.Data.Enums;

namespace DwarfQuest.Components.Buttons;

public partial class ItemButton : ButtonBase
{
    public ActionType ActionType = ActionType.Item;
    
    public ItemButton()
    {
        Text = nameof(ButtonType.Item);
    }
}