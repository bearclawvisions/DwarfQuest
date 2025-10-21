using DwarfQuest.Data.Dto;
using Godot;
using System;

namespace DwarfQuest.Components.Character;

public partial class Combatant : Node2D
{
    public CombatDto CombatInfo;
    private Sprite2D _sprite;
    
    [Export] public string PlayerName = "Player1";
    [Export] public int Speed = 10;
    
    public int Health = 100;
    public int Damage = 10;
    public bool IsPlayer = false;
    public int Round = 0; // set to 1 on Surprised (Enemy) or Backattack (Player) state
    public Vector2 CombatPosition;
    public Vector2 CombatEnteredFrom;

    public bool IsSelected { get; set; }
    public bool IsTarget { get; set; }
    
    private readonly Random _random = new();

    public void Select()
    {
        IsSelected = true;
        Modulate = new Color(1, 0.5f, 0.5f); // highlight red
    }

    public void Deselect()
    {
        IsSelected = false;
        IsTarget = false;
        Modulate = new Color(1, 1, 1); // normal
    }
    
    public void TakeDamage(int damage)
    {
        CombatInfo.Health -= damage;
    }
    
    public void Heal(int heal)
    {
        CombatInfo.Health += heal;
    }

    public virtual void OnDeath()
    {
        Deselect();
    }

    public void EnterCombat()
    {
        CombatPosition = Position;
        var randomY = (float)_random.Next(-100, 100);
        CombatEnteredFrom = IsPlayer ? new Vector2(300, randomY) : new Vector2(-300, randomY);
        Position = CombatPosition + CombatEnteredFrom;

        var tween = GetTree().CreateTween();
        tween.TweenProperty(this, "position", CombatPosition, 1.0f)
            .SetTrans(Tween.TransitionType.Sine)
            .SetEase(Tween.EaseType.Out);

        tween.Finished += () => { Position = CombatPosition; };
    }

    public void SetTexture(Texture2D texture)
    {
        _sprite = new Sprite2D();
        _sprite.Texture = texture;
        AddChild(_sprite);
    }
}