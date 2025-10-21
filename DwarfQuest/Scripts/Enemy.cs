using DwarfQuest.Data.Enums;
using Godot;
using DwarfQuest.Components.Character;

namespace DwarfQuest.Scripts;

public partial class Enemy : Combatant
{
	private Sprite2D _sprite;

	private float _deathDuration = 0.5f;
	private Vector2 _deathPosition = new Vector2(0f, 25.0f);
	
	public override void _Ready()
	{
		_sprite = GetNode<Sprite2D>("Sprite2D");
		
		Damage = 10;
		EnterCombat();
	}

	public override void OnDeath()
	{
		base.OnDeath();
		
		var tween = GetTree().CreateTween();
		tween.Parallel().TweenProperty(_sprite, GodotProperty.ModulateAlpha, 0.0, _deathDuration);
		tween.Parallel().TweenProperty(_sprite, GodotProperty.Scale, Vector2.Right, _deathDuration).SetTrans(Tween.TransitionType.Linear);
		tween.Parallel().TweenProperty(_sprite, GodotProperty.Position, _sprite.Position + _deathPosition, _deathDuration);

		tween.Finished += QueueFree;
	}
}