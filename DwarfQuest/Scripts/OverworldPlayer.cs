using DwarfQuest.Data.Enums;
using Godot;
using System.Collections.Generic;

namespace DwarfQuest.Scripts;

public partial class OverworldPlayer : CharacterBody2D
{
	private const float Speed = 300.0f;
	private const float Deceleration = Speed * 5.0f;
	private OverworldPlayerState _state = OverworldPlayerState.Idle;
	private Facing _facing = Facing.Front;
	private Sprite2D _sprite;
	
	public override void _Ready()
	{
		MotionMode = MotionModeEnum.Floating;
		_sprite = GetNode<Sprite2D>("Sprite");
	}

	public override void _Process(double delta)
	{
		SetAnimation();
	}

	public override void _PhysicsProcess(double delta)
	{
		Vector2 velocity;

		var direction = Input.GetVector(
			nameof(Movement.MoveLeft),
			nameof(Movement.MoveRight),
			nameof(Movement.MoveUp),
			nameof(Movement.MoveDown)
		);
		
		if (direction != Vector2.Zero)
		{
			velocity = direction * Speed;
		}
		else
		{
			velocity = Velocity.MoveToward(Vector2.Zero, (float)delta * Deceleration);
		}
		
		Velocity = velocity;
		MoveAndSlide();
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
		var idleFrameIndex = new List<int> {0, 2};
		_sprite.Frame = idleFrameIndex[0];
	}
}