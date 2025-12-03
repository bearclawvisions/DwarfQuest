using DwarfQuest.Bridge.Extensions;
using DwarfQuest.Bridge.Managers;
using DwarfQuest.Data.Enums;
using Godot;
using System;

namespace DwarfQuest.Scripts;

public partial class OverworldPlayer : CharacterBody2D
{
	private const float Speed = 100.0f;
	private const float Deceleration = Speed * 5;
	private const float DistancePerStep = 16f; // pixels per step
	private static readonly Vector2 SpriteSize = new (48, 48); // lot of empty space
	
	private OverworldPlayerState _state = OverworldPlayerState.Idle;
	private Facing _facing = Facing.Front;
	private AnimatedSprite2D _sprite;
	private CollisionShape2D _collisionShape;
	private float _distanceTraveled;
	private Vector2 _previousPosition;
	
	public OverworldPlayerState State => _state;
	public int StepsTaken { get; private set; }

	public override void _Ready()
	{
		this.ClearPlaceholders();
		
		MotionMode = MotionModeEnum.Floating;
		CollisionLayer = (uint)CollideLayer.Player;
		CollisionMask = (uint)CollideLayer.Walls;
		
		AddSprite();
		AddCollisionShape();
		AddCamera();
	}

	private void AddCollisionShape()
	{
		_collisionShape = new CollisionShape2D();
		AddChild(_collisionShape);
		
		var sizeX = (float)Math.Round(SpriteSize.X * 0.20f, MidpointRounding.AwayFromZero);
		var sizeY = (float)Math.Round(SpriteSize.Y * 0.33f, MidpointRounding.AwayFromZero);
		_collisionShape.Shape = new RectangleShape2D { Size = new Vector2(sizeX, sizeY) };
	}

	public override void _Process(double delta)
	{
		SetAnimation();
	}

	public override void _PhysicsProcess(double delta)
	{
		if (_previousPosition == Vector2.Zero)
			_previousPosition = Position;
		
		Vector2 velocity;
		var direction = GetInputVector2();
		if (direction != Vector2.Zero)
		{
			_state = OverworldPlayerState.Walking;
			velocity = direction * Speed;
			SetFacing(direction);
		}
		else
		{
			_state = OverworldPlayerState.Idle;
			velocity = Velocity.MoveToward(Vector2.Zero, (float)delta * Deceleration);
		}
		
		Velocity = velocity;
		MoveAndSlide();
		
		// after moving calculate steps
		if (direction != Vector2.Zero)
		{
			CalculateSteps();
		}
	}
	
	public void ResetStepCounter()
	{
		StepsTaken = 0;
	}

	private void CalculateSteps()
	{
		var actualDistanceMoved = Position.DistanceTo(_previousPosition);
		_distanceTraveled += actualDistanceMoved;
		_previousPosition = Position;
    
		while (_distanceTraveled >= DistancePerStep)
		{
			StepsTaken++;
			_distanceTraveled -= DistancePerStep;
		}
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
	
	private void AddSprite()
	{
		StepsTaken = 0; // reset steps taken, maybe integrate with service to save TotalStepsTaken
		
		_sprite = new AnimatedSprite2D();
		_sprite.SpriteFrames = ResourceManager.GetAsset<SpriteFrames>(AssetName.OverworldPlayerAnimations);
		AddChild(_sprite);
		SetAnimation();
		_sprite.Play();
	}
	
	private void AddCamera()
	{
		var camera = new Camera2D();
		AddChild(camera);
	}

	private void SetFacing(Vector2 direction)
	{
		if (direction == Vector2.Down) _facing = Facing.Front;
		if (direction == Vector2.Up) _facing = Facing.Back;
		if (direction == Vector2.Left) _facing = Facing.Left;
		if (direction == Vector2.Right) _facing = Facing.Right;
		
		// diagonal facing ??
	}

	private void SetAnimation()
	{
		switch (_state)
		{
			case OverworldPlayerState.Idle: SetIdleAnimation(); break;
			case OverworldPlayerState.Walking: SetWalkingAnimation(); break;
			default:
				throw new ArgumentOutOfRangeException($"Enum out of range {nameof(OverworldPlayerState)}.");
		}
	}

	private void SetWalkingAnimation()
	{
		switch (_facing)
		{
			case Facing.Front: _sprite.Animation = nameof(AnimationName.WalkFront); break;
			case Facing.Back: _sprite.Animation = nameof(AnimationName.WalkBack); break;
			case Facing.Right: _sprite.Animation = nameof(AnimationName.WalkSide); _sprite.FlipH = false; break;
			case Facing.Left: _sprite.Animation = nameof(AnimationName.WalkSide); _sprite.FlipH = true; break;
			default:
				throw new ArgumentOutOfRangeException($"Enum out of range {nameof(Facing)}, while walking.");
		}
	}

	private void SetIdleAnimation()
	{
		switch (_facing)
		{
			case Facing.Front: _sprite.Animation = nameof(AnimationName.IdleFront); break;
			case Facing.Back: _sprite.Animation = nameof(AnimationName.IdleBack); break;
			case Facing.Right: _sprite.Animation = nameof(AnimationName.IdleSide); _sprite.FlipH = false; break;
			case Facing.Left: _sprite.Animation = nameof(AnimationName.IdleSide); _sprite.FlipH = true; break;
			default:
				throw new ArgumentOutOfRangeException($"Enum out of range {nameof(Facing)}, while idle.");
		}
	}
}