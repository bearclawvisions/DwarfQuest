using DwarfQuest.Bridge.Extensions;
using DwarfQuest.Data.Enums;
using DwarfQuest.Data.Extensions;
using DwarfQuest.Data.Models;
using Godot;

namespace DwarfQuest.Components.Container;

public partial class MemberEntry : HBoxContainer
{
    private ColorRect _portrait; // todo actual portrait
    private Label _name;
    private Label _dash;
    private Label _expLabel;
    private ProgressBar _expBar;
    private Label _spLabel;
    private Label _spAmount;
    
    public void Initialize(PlayerBattleResultInfo info)
    {
        SetBase();
        
        _name.Text = info.Name;
        _expBar.Value = info.Experience;
        _expBar.MaxValue = info.ExperienceToNextLevel;
        _spAmount.Text = info.SkillPoints.ToString();
    }
    
    public void UpdateValues(int experience, int skillPoints)
    {
        UpdateExperience(experience);
        UpdateSkillPoints(skillPoints);
    }

    private void UpdateSkillPoints(int skillPoints)
    {
        var currentSp = _spAmount.Text.ToInt();
        var totalSp = currentSp + skillPoints;
        _spAmount.CountUpAnimation(totalSp, currentSp);
    }

    private void UpdateExperience(int experience)
    {
        // todo: on MaxValue, levelup animation and fillbar again for next level
        // popup window to show stat changes?
        var totalExp = _expBar.Value + experience;
        
        var tween = CreateTween();
        tween.TweenProperty(_expBar, GodotProperty.Value, totalExp, 0.5);
    }

    private void SetBase()
    {
        _portrait = new ColorRect();
        _portrait.Color = new Color(0.71f, 0.25f, 0.38f); // reddish color
        _portrait.CustomMinimumSize = new Vector2(25, 25);
        
        _name = new Label { CustomMinimumSize = new Vector2(80, 25)};
        
        _dash = new Label { Text = UiLabels.SpacedDash.GetDescription() };
        
        _expLabel = new Label { Text = UiLabels.Experience.GetDescription() };
        
        _expBar = new ProgressBar { ShowPercentage = false, CustomMinimumSize = new Vector2(75, 10), SizeFlagsVertical = SizeFlags.ShrinkCenter };
        
        _spLabel = new Label { Text = UiLabels.SkillPoints.GetDescription() };
        
        _spAmount = new Label();
        _spAmount.SizeFlagsHorizontal = SizeFlags.ShrinkEnd | SizeFlags.Expand;

        AddChild(_portrait);
        AddChild(_name);
        AddChild(_dash);
        AddChild(_expLabel);
        AddChild(_expBar);
        AddChild(_spLabel);
        AddChild(_spAmount);
    }
}