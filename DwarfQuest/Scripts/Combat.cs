using DwarfQuest.Data.Enums;
using DwarfQuest.Extensions;
using DwarfQuest.Managers;
using Godot;
using System.Threading.Tasks;

namespace DwarfQuest.Scripts;

public partial class Combat : Node
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
		if (_battleManager.State == CombatState.ExitCombat)
			BattleEnded();
	}

	private void InitializeCombatMenu()
	{
		_combatMenu = GetNode<CombatMenu>("CanvasLayer/CombatMenu");
		_combatMenu.FightButton.Pressed += OnFight;
	}

	private void InitializeCombatParticipants()
	{
		_players = GetNode<Players>("%Players");
		_enemies = GetNode<Enemies>("%Enemies");
		
		_battleManager = new BattleManager(_combatMenu, _enemies, _players);
	}
	
	private void InitializeBattleAfterDelay()
	{
		this.AddWaitTimer(_battleManager.StartCombat);
	}
	
	private void OnFight()
	{
		_combatMenu.IsMenuActive = false;
		_combatMenu.FightButton.Disabled = true;
		_enemies.CanSelect = true;
		
		_battleManager.OnActionSelected(ActionType.Fight);
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
			_battleManager.TargetsSelected();
		}
	}

	private void ResetToMenu()
	{
		_combatMenu.Reset();
		_enemies.Reset();
		_players.Reset();
	}
	
	public void BattleEnded()
	{
		GD.Print("Combat ended! Closing scene...");
		// GetTree().ChangeSceneToFile("res://Scenes/Overworld.tscn");
		QueueFree();
		GetTree().Quit();
	}
}