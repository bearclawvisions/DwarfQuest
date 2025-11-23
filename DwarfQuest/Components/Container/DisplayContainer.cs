using DwarfQuest.Data.Enums;
using DwarfQuest.Data.Extensions;
using DwarfQuest.Scripts;
using Godot;
using System;

namespace DwarfQuest.Components.Container;

public partial class DisplayContainer : HBoxContainer
{
    private Label _name;
    private Label _digits;

    private int _amount;

    public void Initialize(UiLabels name, int amount)
    {
        SetBase();
        
        _amount = amount;
        _digits.Text = amount.ToString();
        
        DefineDisplayTypeAndPosition(name);
        CountUpAnimation();
    }

    private void DefineDisplayTypeAndPosition(UiLabels name)
    {
        _name.Text = name.GetShortDescription();
        const float displayHeight = 60f;
        var x = CalculateHorizontalPosition(name);
        Position = new Vector2(x, displayHeight);
    }

    private float CalculateHorizontalPosition(UiLabels name)
    {
        var screenSize = AutoLoader.GetWindowSize();

        switch (name)
        {
            case UiLabels.Experience:
            {
                var center = screenSize.X / 3;
                var sizeOffset = Size.X;
                return center - sizeOffset;
            }
            case UiLabels.SkillPoints:
            {
                var center = screenSize.X / 2;
                var sizeOffset = Size.X / 2;
                return center - sizeOffset;
            }
            case UiLabels.Money:
            {
                var center = screenSize.X / 3;
                return center * 2;
            }
            default:
                return 0;
        }
    }

    private void SetBase()
    {
        Size = new Vector2(150, 50);
        
        _name = new Label();
        _digits = new Label();
        _digits.SizeFlagsHorizontal = SizeFlags.ShrinkEnd | SizeFlags.Expand;
            
        AddChild(_name);
        AddChild(_digits);
    }

    private void CountUpAnimation()
    {
        var tween = CreateTween();
        tween.SetEase(Tween.EaseType.Out);
        tween.SetTrans(Tween.TransitionType.Cubic);
        tween.TweenMethod(Callable.From((double value) => 
        {
            _digits.Text = Mathf.RoundToInt((float)value).ToString();
        }), 0.0, (double)_amount, 1.5f);
    }
}