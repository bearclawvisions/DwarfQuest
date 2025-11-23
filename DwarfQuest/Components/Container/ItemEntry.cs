using DwarfQuest.Data.Models;
using Godot;

namespace DwarfQuest.Components.Container;

public partial class ItemEntry : HBoxContainer
{
    private Label _name;
    private Label _digits;

    public void Initialize(Item item)
    {
        SetBase();
        
        _name.Text = item.Name;
        _digits.Text = item.Amount.ToString();
    }

    private void SetBase()
    {
        _name = new Label();
        _digits = new Label();
        _digits.SizeFlagsHorizontal = SizeFlags.ShrinkEnd | SizeFlags.Expand;
            
        AddChild(_name);
        AddChild(_digits);
    }
}