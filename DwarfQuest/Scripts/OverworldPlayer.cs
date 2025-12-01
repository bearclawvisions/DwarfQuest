using DwarfQuest.Data.Enums;
using Godot;
using System;
using Animation = DwarfQuest.Data.Enums.Animation;

namespace DwarfQuest.Scripts;

public partial class OverworldPlayer : CharacterBody2D
{
	private const float Speed = 300.0f;
	private const float Deceleration = Speed * 5.0f;
	private OverworldPlayerState _state = OverworldPlayerState.Idle;
	private Facing _facing = Facing.Front;
	private AnimatedSprite2D _sprite;
	
	public override void _Ready()
	{
		MotionMode = MotionModeEnum.Floating;
		_sprite = GetNode<AnimatedSprite2D>("Sprite");
	}

	public override void _Process(double delta)
	{
		SetAnimation();
	}

	public override void _PhysicsProcess(double delta)
	{
		Vector2 velocity;
		var direction = GetInputVector2();
		if (direction != Vector2.Zero)
		{
			velocity = direction * Speed;
			SetFacing(direction);
		}
		else
		{
			velocity = Velocity.MoveToward(Vector2.Zero, (float)delta * Deceleration);
		}
		
		Velocity = velocity;
		MoveAndSlide();
	}

	private Vector2 GetInputVector2()
	{
		return Input.GetVector(
			nameof(Movement.MoveLeft),
			nameof(Movement.MoveRight),
			nameof(Movement.MoveUp),
			nameof(Movement.MoveDown)
		);
	}

	private void SetFacing(Vector2 direction)
	{
		if (direction == Vector2.Down) _facing = Facing.Front;
		if (direction == Vector2.Up) _facing = Facing.Back;
		if (direction == Vector2.Left) _facing = Facing.Left;
		if (direction == Vector2.Right) _facing = Facing.Right;
	}

	private void SetAnimation()
	{
		switch (_state)
		{
			case OverworldPlayerState.Idle: SetIdleAnimation(); break;
			// case OverworldPlayerState.Walking: SetWalkingAnimation(); break;
		}
	}

	private void SetIdleAnimation()
	{
		_sprite.FlipH = false;
		
		switch (_facing)
		{
			case Facing.Front:
				_sprite.Animation = nameof(Animation.IdleFront);
				break;
			case Facing.Back:
				_sprite.Animation = nameof(Animation.IdleBack);
				break;
			case Facing.Right:
				_sprite.Animation = nameof(Animation.IdleSide);
				break;
			case Facing.Left:
				_sprite.Animation = nameof(Animation.IdleSide);
				_sprite.FlipH = true;
				break;
			default:
				throw new ArgumentOutOfRangeException($"Enum out of range {nameof(Facing)}");
		}
		
		_sprite.Play();
	}
}