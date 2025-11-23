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
		
		SetBattleResultData();
		CountUpAnimation();
	}

	private void SetBattleResultData()
	{
		_expAmount.Text = _result.Experience.ToString();
		_skillPointsAmount.Text = _result.SkillPoints.ToString();
		_moneyAmount.Text = _result.Money.ToString();
	}
	
	private void CountUpAnimation()
	{
		CreateCountUpTween(_expAmount, _result.Experience, 1.5f);
		CreateCountUpTween(_skillPointsAmount, _result.SkillPoints, 1.5f);
		CreateCountUpTween(_moneyAmount, _result.Money, 1.5f);
	}

	private void CreateCountUpTween(Label label, int targetValue, float duration)
	{
		var tween = CreateTween();
		tween.SetEase(Tween.EaseType.Out);
		tween.SetTrans(Tween.TransitionType.Cubic);
		tween.TweenMethod(Callable.From((double value) => 
		{
			label.Text = Mathf.RoundToInt((float)value).ToString();
		}), 0.0, (double)targetValue, duration);
	}

	// use input to close and go to overworld
}