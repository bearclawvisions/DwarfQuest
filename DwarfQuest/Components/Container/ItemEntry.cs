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
        Size = new Vector2(250, 23);
        
        _name = new Label();
        _digits = new Label();
        _digits.SizeFlagsHorizontal = SizeFlags.ShrinkEnd | SizeFlags.Expand;
            
        AddChild(_name);
        AddChild(_digits);
    }

    // private void CountUpAnimation()
    // {
    //     var tween = CreateTween();
    //     tween.SetEase(Tween.EaseType.Out);
    //     tween.SetTrans(Tween.TransitionType.Cubic);
    //     tween.TweenMethod(Callable.From((double value) => 
    //     {
    //         _digits.Text = Mathf.RoundToInt((float)value).ToString();
    //     }), 0.0, (double)_amount, 1.5f);
    // }
}