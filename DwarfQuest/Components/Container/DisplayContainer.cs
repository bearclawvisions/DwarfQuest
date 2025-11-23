using DwarfQuest.Data.Enums;
using Godot;
using System;

namespace DwarfQuest.Components.Container;

public partial class DisplayContainer : HBoxContainer
{
    private Label _name;
    private Label _digits;

    private int _amount;

    public void Initialize(DisplayEnum name, int amount)
    {
        SetBase();
        
        _amount = amount;
        
        DefineDisplayTypeAndPosition(name);
        _digits.Text = amount.ToString();
        
        CountUpAnimation();
    }

    private void DefineDisplayTypeAndPosition(DisplayEnum name)
    {
        _name.Text = name.ToString();
        const float displayHeight = 60f;

        Position = name switch
        {
            DisplayEnum.Experience => new Vector2(40f, displayHeight),
            DisplayEnum.SkillPoints => new Vector2(245f, displayHeight),
            DisplayEnum.Money => new Vector2(450f, displayHeight),
            _ => throw new ArgumentOutOfRangeException(nameof(name), name, null)
        };
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