using DwarfQuest.Bridge.Extensions;
using DwarfQuest.Bridge.Managers;
using DwarfQuest.Business.Implementation;
using DwarfQuest.Components.Container;
using DwarfQuest.Data.Dto;
using DwarfQuest.Data.Enums;
using DwarfQuest.Data.Models;
using Godot;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DwarfQuest.Scripts;

public partial class CombatResults : Control
{
	private readonly CombatService _combatService = GameManager.CombatService;
	private BattleResult _result;
	private List<PlayerBattleResultDto> _playerResults;
	
	private DisplayContainer _expContainer;
	private DisplayContainer _skillPointContainer;
	private DisplayContainer _moneyContainer;

	private VBoxContainer _itemContainer;
	
	public override void _Ready()
	{
		var viewport = AutoLoader.GetWindowSize();
		
		_itemContainer = GetNode<VBoxContainer>("%ItemContainer");
		_itemContainer.ClearPlaceholders();

		_result = _combatService.GetBattleResult();
		_playerResults = _combatService.GetPlayerPartyForBattleResults();
		
		SetBattleResultData();
		_ = ShowItemsOneByOne();
		ShowPartyGaugeIncreases();
	}

	private void SetBattleResultData()
	{
		_expContainer = new DisplayContainer();
		_expContainer.Initialize(DisplayEnum.Experience, _result.Experience);
		AddChild(_expContainer);
		
		_skillPointContainer = new DisplayContainer();
		_skillPointContainer.Initialize(DisplayEnum.SkillPoints, _result.SkillPoints);
		AddChild(_skillPointContainer);
		
		_moneyContainer = new DisplayContainer();
		_moneyContainer.Initialize(DisplayEnum.Money, _result.Money);
		AddChild(_moneyContainer);
	}
	
	private async Task ShowItemsOneByOne()
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
			
			itemEntry.Modulate = new Color(1, 1, 1, 0);
			_itemContainer.AddChild(itemEntry);
		
			var tween = CreateTween();
			tween.TweenProperty(itemEntry, GodotProperty.ModulateAlpha, 1.0, 0.2);
		
			await ToSignal(GetTree().CreateTimer(0.2), SceneTreeTimer.SignalName.Timeout);
		}
	}
	
	private void ShowPartyGaugeIncreases()
	{
		
	}

	// use input to close and go to overworld
}