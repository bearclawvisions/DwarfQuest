using Godot;
using System;

namespace DwarfQuest.Components.Character;

public partial class CharacterBase : Node2D
{
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
    public bool IsDead => Health <= 0;
    
    private Random _random = new Random();

    public void Select()
    {
        IsSelected = true;
        Modulate = new Color(1, 0.5f, 0.5f); // highlight red
    }

    public void Deselect()
    {
        IsSelected = false;
        Modulate = new Color(1, 1, 1); // normal
    }
    
    public void TakeDamage(int damage)
    {
        Health -= damage;
    }
    
    public void Heal(int heal)
    {
        Health += heal;
    }

    public virtual void OnDeath()
    {
        Deselect();
    }

    protected void EnterCombat()
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
}