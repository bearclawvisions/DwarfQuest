using DwarfQuest.Bridge.Extensions;
using DwarfQuest.Bridge.Managers;
using DwarfQuest.Business.Interfaces;
using DwarfQuest.Data.Dto;
using DwarfQuest.Data.Enums;
using Godot;
using System.Threading.Tasks;

namespace DwarfQuest.Scripts;

public partial class Combat : Node, ICombatEventListener
{
	// Godot
	private CombatMenu _combatMenu;
	private Enemies _enemies;
	private Players _players;
	
	private BattleManager _battleManager;
	
	public override void _Ready()
	{
		InitializeCombatMenu();
		InitializeCombatParticipants(); // and BattleManager
		InitializeBattleAfterDelay();
	}

	public override void _Process(double delta)
	{
		_combatMenu.IsMenuActive = _battleManager.State >= CombatState.AwaitingPlayerInput;
		
		if (_battleManager.State == CombatState.NewTurn)
			ResetToMenu();
		
		if (_battleManager.State == CombatState.ExitCombat)
			BattleEnded();
	}
	
	public async Task ShowMessageAsync(string message)
	{
		GD.Print(message);
		await Task.Delay(500); // simulate ui delay
	}

	public async Task CombatantDeathAsync(CombatDto combatant)
	{
		if (!combatant.IsPlayer)
		{
			_enemies.RemoveEnemy(combatant);
		}
		await Task.CompletedTask;
	}

	public async Task PlayAttackAnimationAsync()
	{
		// var animPlayer = attackerId == "Player" ? _playerAnimation : _enemyAnimation;
		//
		// var animationName = "Attack";
		// animPlayer.Play(animationName);
		//
		// // Wait for animation to finish using Godot’s Signal-to-Task
		// var tcs = new TaskCompletionSource();
		// void OnAnimationFinished(StringName name)
		// {
		// 	if (name == animationName)
		// 	{
		// 		animPlayer.AnimationFinished -= OnAnimationFinished;
		// 		tcs.SetResult();
		// 	}
		// }
		// animPlayer.AnimationFinished += OnAnimationFinished;
		//
		// await tcs.Task;
		
		GD.Print("Simulating attack animation...");
		await Task.Delay(300);
	}

	private void InitializeCombatMenu()
	{
		_combatMenu = GetNode<CombatMenu>("CanvasLayer/CombatMenu");
		_combatMenu.FightButton.Pressed += OnFight;
	}

	private void InitializeCombatParticipants()
	{
		_battleManager = new BattleManager(this);
		
		_players = GetNode<Players>("%Players");
		_players.InitializeParty(_battleManager.Players);
		
		_enemies = GetNode<Enemies>("%Enemies");
		_enemies.InitializeParty(_battleManager.Enemies);
	}
	
	private void InitializeBattleAfterDelay()
	{
		this.AddWaitTimer(_battleManager.StartCombat);
	}
	
	private void OnFight()
	{
		_combatMenu.FightButton.Disabled = true;
		_enemies.CanSelect = true;
		
		_battleManager.OnActionSelected(_combatMenu.FightButton.ActionType);
	}
	
	public override void _UnhandledInput(InputEvent @event)
	{
		if (_battleManager.State == CombatState.HandleAnimation) return;
		
		if (@event is InputEventKey { Pressed: true, Keycode: Key.Backspace } && !_combatMenu.IsMenuActive)
			ResetToMenu();

		if (@event is InputEventKey { Pressed: true, Keycode: Key.D } && !_combatMenu.IsMenuActive)
		{
			_enemies.CanSelect = false;
			_players.CanSelect = true;
		}
		
		if (@event is InputEventKey { Pressed: true, Keycode: Key.A } && !_combatMenu.IsMenuActive)
		{
			_enemies.CanSelect = true;
			_players.CanSelect = false;
		}
		
		if (@event is InputEventKey { Pressed: true, Keycode: Key.Enter } && (_enemies.CanSelect || _players.CanSelect))
		{
			GD.Print("Target selected pressed!");
			_ = _battleManager.TargetsSelected();
		}
	}

	private void ResetToMenu()
	{
		_combatMenu.EnableButtons();
		_enemies.Reset();
		_players.Reset();
		_battleManager.OnActionCancelled();
	}

	private void BattleEnded()
	{
		GD.Print("Combat ended! Closing scene...");
		// GetTree().ChangeSceneToFile("res://Scenes/Overworld.tscn");
		QueueFree();
		GetTree().Quit();
	}
}