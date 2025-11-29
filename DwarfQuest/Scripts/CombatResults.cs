using DwarfQuest.Bridge.Extensions;
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
	
	private ItemContainer _itemContainer;
	private PartyContainer _partyContainer;
	
	public override void _Ready()
	{
		this.ClearPlaceholders();
		_result = _combatService.GetBattleResult();
		_playerResults = _combatService.GetPlayerPartyForBattleResults();
		
		InitializeStaticText();
		SetBattleResultData();
		_ = LoadItems();
		LoadParty();
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
		CreateDisplayContainer(UiLabels.Experience, _result.Experience);
		CreateDisplayContainer(UiLabels.SkillPoints, _result.SkillPoints);
		CreateDisplayContainer(UiLabels.Money, _result.Money);
	}

	private void CreateDisplayContainer(UiLabels label, int value)
	{
		var container = new DisplayContainer();
		container.Initialize(label, value);
		AddChild(container);
	}
	
	private async Task LoadItems()
	{
		_itemContainer = new ItemContainer();
		AddChild(_itemContainer);
		await _itemContainer.Initialize(_result.Items);
	}
	
	private void LoadParty()
	{
		_partyContainer = new PartyContainer();
		AddChild(_partyContainer);
		_partyContainer.Initialize(_playerResults, _result);
	}

	// use input to close and go to overworld
}