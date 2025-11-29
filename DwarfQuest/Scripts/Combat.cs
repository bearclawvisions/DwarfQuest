using DwarfQuest.Bridge.Extensions;
using DwarfQuest.Bridge.Managers;
using DwarfQuest.Business.Interfaces;
using DwarfQuest.Data.Enums;
using DwarfQuest.Data.Models;
using Godot;
using System.Threading.Tasks;

namespace DwarfQuest.Scripts;

public partial class Combat : Node, ICombatEventListener
{
	private CombatMenu _combatMenu;
	private RunMenu _runMenu;
	
	private PlayerInfo _playerInfo;
	private Enemies _enemies;
	private Players _players;
	
	private BattleManager _battleManager;
	
	public override void _Ready()
	{
		_battleManager = new BattleManager(this);
		_runMenu = new RunMenu();
		AddChild(_runMenu);
		
		InitializeCombatMenu();
		InitializeCombatParticipants();
		InitializeBattleAfterDelay();
		InitializePlayerInfo();
	}

	public override void _Process(double delta)
	{
		_combatMenu.IsMenuActive = _battleManager.State >= CombatState.AwaitingPlayerInput;
		_enemies.CanSelect = _battleManager.State == CombatState.TargetSelectionEnemy;
		_players.CanSelect = _battleManager.State == CombatState.TargetSelectionPlayer;
		
		if (_battleManager.State == CombatState.PlayerTurn)
			ResetToMenu();
		
		if (_battleManager.State == CombatState.ExitCombat)
			BattleEnded();
		
		if (_battleManager.State == CombatState.Run)
		{
			_runMenu.ShowRunMenu();
			// run away animation
			// BattleEnded(true);
		}
	}
		
	public override void _UnhandledInput(InputEvent @event)
	{
		if (_battleManager.State == CombatState.HandleAnimation)
			return;
		
		if (@event is InputEventKey { Pressed: true, Keycode: Key.Backspace } && !_combatMenu.IsMenuActive)
			ResetToMenu();

		if (@event is InputEventKey { Pressed: true, Keycode: Key.D } && !_combatMenu.IsMenuActive)
			_battleManager.OnSwitchTargetSelection(Target.Player);
		
		if (@event is InputEventKey { Pressed: true, Keycode: Key.A } && !_combatMenu.IsMenuActive)
			_battleManager.OnSwitchTargetSelection(Target.Enemy);
		
		if (@event is InputEventKey { Pressed: true, Keycode: Key.Enter } && (_enemies.CanSelect || _players.CanSelect))
			_ = _battleManager.TargetsSelected();
	}
	
	public async Task ShowMessageAsync(string message)
	{
		GD.Print(message);
		await Task.Delay(300); // simulate ui delay
	}
	
	public void UpdatePlayerInfo(CombatantInfo combatant)
	{
		_playerInfo.UpdateHealth(combatant);
	}

	public async Task CombatantDeathAsync(CombatantInfo combatant)
	{
		if (!combatant.IsPlayer)
			_enemies.RemoveParticipant(combatant);
		
		await Task.CompletedTask;
	}

	public async Task PlayAttackAnimationAsync()
	{
		GD.Print("Simulating attack animation...");
		await Task.Delay(300);
	}

	private void InitializeCombatMenu()
	{
		_combatMenu = GetNode<CombatMenu>("CanvasLayer/CombatMenu");
		_combatMenu.FightButton.Pressed += OnFightPressed;
		_combatMenu.TacticButton.Pressed += OnTacticPressed;
		_combatMenu.ItemButton.Pressed += OnItemPressed;
		_combatMenu.RunButton.Pressed += OnRunPressed;
	}

	private void InitializeCombatParticipants()
	{
		_players = GetNode<Players>("%Players");
		_players.InitializeParty(_battleManager.Players);
		
		_enemies = GetNode<Enemies>("%Enemies");
		_enemies.InitializeParty(_battleManager.Enemies);
	}
	
	private void InitializePlayerInfo()
	{
		_playerInfo = GetNode<PlayerInfo>("CanvasLayer/PlayerInfo");
		_playerInfo.InitializePlayerInfo(_battleManager.Players);
	}
	
	private void InitializeBattleAfterDelay()
	{
		this.AddWaitTimer(_battleManager.StartCombat);
	}
	
	private void OnFightPressed()
	{
		_combatMenu.FightButton.Disabled = true;
		_enemies.CanSelect = true;
		
		_battleManager.OnActionSelected(_combatMenu.FightButton.ActionType);
	}
	
	private void OnTacticPressed()
	{
		_combatMenu.TacticButton.Disabled = true;
		// _enemies.CanSelect = true; // maybe
		
		_battleManager.OnActionSelected(_combatMenu.TacticButton.ActionType);
	}
	
	private void OnItemPressed()
	{
		_combatMenu.ItemButton.Disabled = true;
		
		_battleManager.OnActionSelected(_combatMenu.ItemButton.ActionType);
	}
	
	private void OnRunPressed()
	{
		_combatMenu.RunButton.Disabled = true;
		// popup yes/no to flee
		
		_battleManager.OnActionSelected(_combatMenu.RunButton.ActionType);
	}

	private void ResetToMenu()
	{
		_combatMenu.EnableButtons();
		_enemies.Reset();
		_players.Reset();
		_battleManager.OnActionCancelled();
	}

	private void BattleEnded(bool runaway = false)
	{
		if (runaway)
		{
			GetTree().Quit(); // change to overworld directly
			// this.ChangeScene(SceneType.Overworld);
		}
		
		this.ChangeScene(SceneType.CombatResults);
	}
}