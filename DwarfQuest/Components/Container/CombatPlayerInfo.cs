using DwarfQuest.Bridge.Managers;
using DwarfQuest.Data.Dto;
using DwarfQuest.Data.Enums;
using Godot;

namespace DwarfQuest.Components.Container;

public partial class CombatPlayerInfo : HBoxContainer
{
    public int Id { get; private set; }

    private Label _label;
    private ProgressBar _healthBar;

    private CombatPlayerInfo()
    {
        SizeFlagsHorizontal = SizeFlags.ShrinkEnd;
        
        Id = 0;
        _label = new Label();
        _healthBar = new ProgressBar();
        
        AddChild(_label);
        AddChild(_healthBar);
    }

    public static CombatPlayerInfo Create(CombatDto info)
    {
        var component = new CombatPlayerInfo();
        component.Initialize(info);
        return component;
    }

    private void Initialize(CombatDto info)
    {
        Id = info.Id;
        
        _label.Text = info.Name;
        
        _healthBar.ShowPercentage = true;
        _healthBar.CustomMinimumSize = new Vector2(75, 0);
        _healthBar.FillMode = (int)ProgressBar.FillModeEnum.BeginToEnd;
        _healthBar.MinValue = 0;
        _healthBar.MaxValue = info.MaxHealth;
        _healthBar.Step = 1.0;
        _healthBar.Value = info.Health;
    }

    public void UpdateHealth(int currentHealth, int maxHealth)
    {
        _healthBar.MaxValue = maxHealth;
        _healthBar.Value = currentHealth;
    }
}