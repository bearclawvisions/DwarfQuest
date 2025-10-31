using DwarfQuest.Bridge.Managers;
using DwarfQuest.Data.Enums;
using Godot;

namespace DwarfQuest.Components.Buttons;

public partial class CombatButton : Button
{
    // private TextureRect _arrow;
    public ActionType ActionType = ActionType.None;
    
    public CombatButton()
    {
        Theme = ResourceManager.GetAsset<Theme>(AssetName.CombatTheme);
        FocusMode = FocusModeEnum.All;
        Pressed += OnButtonPressed;

        // _arrow.Visible = false; // hide by default
    }
    
    protected virtual void OnButtonPressed()
    {
        if (Disabled) return;
        
        GD.Print($"Button '{Text}' was pressed!");
    }
}