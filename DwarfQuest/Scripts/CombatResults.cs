using DwarfQuest.Bridge.Managers;
using DwarfQuest.Business.Implementation;
using DwarfQuest.Data.Models;
using Godot;

namespace DwarfQuest.Scripts;

public partial class CombatResults : Control
{
	private readonly CombatService _combatService = GameManager.CombatService;
	private BattleResult _result;
	
	public override void _Ready()
	{
		_result = _combatService.GetBattleResult();
		
		GD.Print($"exp: {_result.Experience} - money: {_result.Money} - message: {_result.Message}");
	}
	
	// use input to close and go to overworld
}