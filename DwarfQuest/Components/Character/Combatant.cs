using DwarfQuest.Bridge.Extensions;
using DwarfQuest.Data.Enums;
using DwarfQuest.Data.Models;
using Godot;
using System;

namespace DwarfQuest.Components.Character;

public partial class Combatant : Node2D
{
    public CombatantInfo CombatInfo;
    
    private Sprite2D _sprite;
    private float _enterCombatDuration = 1.0f;
    private float _deathDuration = 0.5f;
    private Vector2 _deathPosition = new Vector2(0f, 25.0f);
    
    private readonly Random _random = new();

    public override void _Process(double delta)
    {
        if (CombatInfo.Health <= 0 && !CombatInfo.IsDead)
            OnDeath();
    }

    public void Select()
    {
        CombatInfo.IsSelected = true;
        Modulate = new Color(1, 0.5f, 0.5f); // highlight red
    }

    public void Deselect()
    {
        CombatInfo.IsSelected = false;
        Modulate = new Color(1, 1, 1); // normal
    }

    private void OnDeath()
    {
        Deselect();
        CombatInfo.IsDead = true;
        
        if (CombatInfo.IsPlayer) return;
        
        // Monster death animation
        var tween = GetTree().CreateTween();
        tween.Parallel().TweenProperty(_sprite, GodotProperty.ModulateAlpha, 0.0, _deathDuration);
        tween.Parallel().TweenProperty(_sprite, GodotProperty.Scale, Vector2.Right, _deathDuration).SetTrans(Tween.TransitionType.Linear);
        tween.Parallel().TweenProperty(_sprite, GodotProperty.Position, _sprite.Position + _deathPosition, _deathDuration);

        tween.Finished += QueueFree;
    }

    public void EnterCombat()
    {
        var enterSideX = CombatInfo.IsPlayer ? 300 : -300; // either enter from the left or right side
        var combatPosition = CombatInfo.CombatPosition.ToGodotVector();
        var randomY = (float)_random.Next(-100, 100);
        var combatEnteredFrom = new Vector2(enterSideX, randomY);
        Position = combatPosition + combatEnteredFrom;

        var tween = GetTree().CreateTween();
        tween.TweenProperty(this, GodotProperty.Position, combatPosition, _enterCombatDuration)
            .SetTrans(Tween.TransitionType.Sine)
            .SetEase(Tween.EaseType.Out);

        tween.Finished += () => { Position = combatPosition; };
    }

    public void SetTexture(Texture2D texture)
    {
        _sprite = new Sprite2D();
        _sprite.Texture = texture;
        AddChild(_sprite);
    }
}