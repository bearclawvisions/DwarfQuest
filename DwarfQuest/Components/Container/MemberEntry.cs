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
    private Label _skillPointAmount;

    public void Initialize(PlayerBattleResultInfo info)
    {
        SetBase();
        
        _name.Text = info.Name;
        _expBar.Value = info.Experience;
        _skillPointAmount.Text += " " + info.SkillPoints;
    }
    
    public void UpdateValues(int experience, int skillPoints)
    {
        _expBar.Value = experience;
        _skillPointAmount.Text = $"{UiLabels.SkillPoints.GetDescription()} {skillPoints}";
    }

    private void SetBase()
    {
        _portrait = new ColorRect();
        _portrait.Color = new Color(0.71f, 0.25f, 0.38f); // reddish
        _portrait.CustomMinimumSize = new Vector2(25, 25);
        
        _name = new Label { CustomMinimumSize = new Vector2(80, 25)};
        
        _dash = new Label { Text = UiLabels.SpacedDash.GetDescription() };
        
        _expLabel = new Label { Text = UiLabels.Experience.GetDescription() };
        
        _expBar = new ProgressBar { ShowPercentage = false, CustomMinimumSize = new Vector2(75, 10), SizeFlagsVertical = SizeFlags.ShrinkCenter };
        
        _skillPointAmount = new Label { Text = UiLabels.SkillPoints.GetDescription() };
        _skillPointAmount.SizeFlagsHorizontal = SizeFlags.ShrinkEnd | SizeFlags.Expand;

        AddChild(_portrait);
        AddChild(_name);
        AddChild(_dash);
        AddChild(_expLabel);
        AddChild(_expBar);
        AddChild(_skillPointAmount);
    }
}