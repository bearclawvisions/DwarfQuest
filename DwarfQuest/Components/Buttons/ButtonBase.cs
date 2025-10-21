using DwarfQuest.Bridge.Managers;
using DwarfQuest.Data.Enums;
using Godot;

namespace DwarfQuest.Components.Buttons;

public partial class ButtonBase : Button
{
    // private TextureRect _arrow;
    
    protected ButtonBase()
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