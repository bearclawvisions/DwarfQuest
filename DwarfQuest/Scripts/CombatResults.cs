using DwarfQuest.Bridge.Managers;
using DwarfQuest.Business.Implementation;
using DwarfQuest.Components.Container;
using DwarfQuest.Data.Enums;
using DwarfQuest.Data.Extensions;
using DwarfQuest.Data.Models;
using Godot;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DwarfQuest.Scripts;

public partial class CombatResults : Control
{
	private readonly CombatService _combatService = GameManager.CombatService;
	private BattleResult _result;
	private List<PlayerBattleResultInfo> _playerResults;
	
	private DisplayContainer _expContainer;
	private DisplayContainer _skillPointContainer;
	private DisplayContainer _moneyContainer;

	private ItemContainer _itemContainer;
	
	public override void _Ready()
	{
		// this.ClearPlaceholders();
		InitializeStaticText();
		
		_result = _combatService.GetBattleResult();
		_playerResults = _combatService.GetPlayerPartyForBattleResults();
		
		SetBattleResultData();
		_ = LoadItems();
		ShowPartyGaugeIncreases();
	}

	private void InitializeStaticText()
	{
		var windowSize = AutoLoader.GetWindowSize();
		var quarterWindowSizeX = windowSize.X * 0.25f;
		var titleHeightPosition = windowSize.Y * 0.07f; // 7% of the screen height
		var labelHeightPosition = windowSize.Y * 0.33f;
		
		CreateStaticLabel(UiLabels.BattleResults.GetDescription(), quarterWindowSizeX * 2, titleHeightPosition);
		CreateStaticLabel(UiLabels.Items.GetDescription(), quarterWindowSizeX, labelHeightPosition);
		CreateStaticLabel(UiLabels.Party.GetDescription(), quarterWindowSizeX * 3, labelHeightPosition);
	}
	
	private void CreateStaticLabel(string text, float xPosition, float yPosition)
	{
		var label = new Label { Text = text };
		AddChild(label);
		var correctedX = xPosition - label.Size.X / 2;
		label.Position = new Vector2(correctedX, yPosition);
	}

	private void SetBattleResultData()
	{
		_expContainer = new DisplayContainer();
		_expContainer.Initialize(UiLabels.Experience, _result.Experience);
		AddChild(_expContainer);
		
		_skillPointContainer = new DisplayContainer();
		_skillPointContainer.Initialize(UiLabels.SkillPoints, _result.SkillPoints);
		AddChild(_skillPointContainer);
		
		_moneyContainer = new DisplayContainer();
		_moneyContainer.Initialize(UiLabels.Money, _result.Money);
		AddChild(_moneyContainer);
	}
	
	private async Task LoadItems()
	{
		_itemContainer = new ItemContainer();
		AddChild(_itemContainer);
		await _itemContainer.Initialize(_result.Items);
	}
	
	private void ShowPartyGaugeIncreases()
	{
		
	}

	// use input to close and go to overworld
}