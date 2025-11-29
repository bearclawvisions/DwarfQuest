using DwarfQuest.Bridge.Managers;
using DwarfQuest.Business.Implementation;
using DwarfQuest.Components.Container;
using DwarfQuest.Data.Dto;
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
	private List<PlayerBattleResultDto> _playerResults;
	
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
		
		var title = new Label();
		title.Text = UiLabels.BattleResults.GetDescription();
		AddChild(title);
		var centerLocation = quarterWindowSizeX * 2 - title.Size.X / 2;
		title.Position = new Vector2(centerLocation, titleHeightPosition);
		
		var itemTitle = new Label();
		itemTitle.Text = UiLabels.Items.GetDescription();
		AddChild(itemTitle);
		var leftLocation = quarterWindowSizeX - itemTitle.Size.X / 2;
		itemTitle.Position = new Vector2(leftLocation, labelHeightPosition);
		
		var partyTitle = new Label();
		partyTitle.Text = UiLabels.Party.GetDescription();
		AddChild(partyTitle);
		var rightLocation = quarterWindowSizeX * 3 - partyTitle.Size.X / 2;
		partyTitle.Position = new Vector2(rightLocation, labelHeightPosition);
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