using DwarfQuest.Bridge.Managers;
using DwarfQuest.Data.Enums;
using DwarfQuest.Data.Models;
using Godot;

namespace DwarfQuest.Components.Container;

public partial class CombatPlayerInfo : HBoxContainer
{
    public int Id { get; private set; }

    private Label _label;
    private Label _healthLabel;
    private ProgressBar _healthBar;

    private CombatPlayerInfo()
    {
        SizeFlagsHorizontal = SizeFlags.ShrinkEnd; // right justified
        
        Id = 0;
        _label = new Label();
        
        _healthLabel = new Label
        {
            LayoutMode = 1, // anchors mode, no exposed enum in api
            AnchorsPreset = (int)LayoutPreset.Center
        };
        
        _healthBar = new ProgressBar
        {
            ShowPercentage = false,
            CustomMinimumSize = new Vector2(100, 25),
            FillMode = (int)ProgressBar.FillModeEnum.BeginToEnd,
            MinValue = 0,
            Step = 1.0,
        };
        
        AddChild(_label);
        AddChild(_healthBar);
        _healthBar.AddChild(_healthLabel);
    }

    public static CombatPlayerInfo Create(CombatantInfo info)
    {
        var component = new CombatPlayerInfo();
        component.Initialize(info);
        return component;
    }

    private void Initialize(CombatantInfo info)
    {
        Id = info.Id;
        _label.Text = info.Name;
        UpdateHealth(info.Health, info.MaxHealth);
    }

    public void UpdateHealth(int currentHealth, int maxHealth)
    {
        _healthBar.MaxValue = maxHealth;
        _healthBar.Value = currentHealth;
        _healthLabel.Text = $"{currentHealth}/{maxHealth}";
    }
}