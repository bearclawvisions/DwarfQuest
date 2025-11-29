using DwarfQuest.Data.Enums;
using DwarfQuest.Data.Extensions;
using DwarfQuest.Scripts;
using Godot;

namespace DwarfQuest.Components.Container;

public partial class DisplayContainer : HBoxContainer
{
    private const float ContainerWidth = 150f;
    private const float ContainerHeight = 50f;
    private const float CountDuration = 1.5f;
    
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
        _name.Text = name.GetDescription();
        var x = CalculateHorizontalPosition(name);
        Position = new Vector2(x, ContainerHeight);
    }

    private float CalculateHorizontalPosition(UiLabels name)
    {
        var screenWidth = AutoLoader.GetWindowSize().X;

        switch (name)
        {
            case UiLabels.Experience:
            {
                var horizontalLocation = screenWidth * 0.33f; // 1/3 of the screen width
                var sizeOffset = Size.X;
                return horizontalLocation - sizeOffset;
            }
            case UiLabels.SkillPoints:
            {
                var center = screenWidth * 0.5f;
                var sizeOffset = Size.X / 2; // to center the label
                return center - sizeOffset;
            }
            case UiLabels.Money:
            {
                var horizontalLocation = screenWidth * 0.33f;
                return horizontalLocation * 2;
            }
            default:
                return 0;
        }
    }

    private void SetBase()
    {
        Size = new Vector2(ContainerWidth, ContainerHeight);
        
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
        }), 0.0, (double)_amount, CountDuration);
    }
}