using DwarfQuest.Bridge.Extensions;
using DwarfQuest.Bridge.Managers;
using DwarfQuest.Business.Implementation;
using DwarfQuest.Data.Dto;
using DwarfQuest.Data.Models;
using Godot;
using System.Collections.Generic;

namespace DwarfQuest.Scripts;

public partial class CombatResults : Control
{
	private readonly CombatService _combatService = GameManager.CombatService;
	private BattleResult _result;
	private List<PlayerBattleResultDto> _playerResults;
	
	private Label _expAmount;
	private Label _skillPointsAmount;
	private Label _moneyAmount;
	private VBoxContainer _itemContainer;
	
	public override void _Ready()
	{
		_expAmount = GetNode<Label>("%ExpAmount");
		_skillPointsAmount = GetNode<Label>("%SkillPointsAmount");
		_moneyAmount = GetNode<Label>("%MoneyAmount");
		
		_itemContainer = GetNode<VBoxContainer>("%ItemContainer");
		_itemContainer.ClearPlaceholders();

		_result = _combatService.GetBattleResult();
		_playerResults = _combatService.GetPlayerPartyForBattleResults();
		
		SetBattleResultData();
		CountUpAnimation();
		ShowItemsOneByOne();
		ShowPartyGaugeIncreases();
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
	
	private void ShowItemsOneByOne()
	{
		foreach (var item in _result.Items)
		{
			var itemEntry = new HBoxContainer();
			var itemName = new Label();
			var itemAmount = new Label();
			itemAmount.SizeFlagsHorizontal = SizeFlags.ShrinkEnd | SizeFlags.Expand;
			
			itemName.Text = item.Name;
			itemAmount.Text = item.Amount.ToString();
			
			itemEntry.AddChild(itemName);
			itemEntry.AddChild(itemAmount);
			
			_itemContainer.AddChild(itemEntry);
		}
	}
	
	private void ShowPartyGaugeIncreases()
	{
		
	}

	// use input to close and go to overworld
}