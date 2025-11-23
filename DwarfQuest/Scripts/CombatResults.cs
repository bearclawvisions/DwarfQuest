using DwarfQuest.Bridge.Managers;
using DwarfQuest.Business.Implementation;
using DwarfQuest.Data.Models;
using Godot;

namespace DwarfQuest.Scripts;

public partial class CombatResults : Control
{
	private readonly CombatService _combatService = GameManager.CombatService;
	private BattleResult _result;
	
	private Label _expAmount;
	private Label _skillPointsAmount;
	private Label _moneyAmount;
	
	public override void _Ready()
	{
		_expAmount = GetNode<Label>("%ExpAmount");
		_skillPointsAmount = GetNode<Label>("%SkillPointsAmount");
		_moneyAmount = GetNode<Label>("%MoneyAmount");
		
		_result = _combatService.GetBattleResult();
		
		_expAmount.Text = _result.Experience.ToString();
		_skillPointsAmount.Text = "2";
		_moneyAmount.Text = _result.Money.ToString();
	}
	
	// use input to close and go to overworld
}