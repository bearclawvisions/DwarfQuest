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
		var windowSizeCenter = AutoLoader.GetWindowSize().X / 2;
		
		var title = new Label();
		title.Text = UiLabels.BattleResults.GetDescription();
		AddChild(title);
		var labelSize = title.Size.X;
		var centerLocation = windowSizeCenter - labelSize / 2;
		title.Position = new Vector2(centerLocation, 25f);
		
		var itemTitle = new Label();
		itemTitle.Text = UiLabels.Items.GetDescription();
		AddChild(itemTitle);
		var leftLocation = windowSizeCenter / 2 - itemTitle.Size.X / 2;
		itemTitle.Position = new Vector2(leftLocation, 120f);
		
		var partyTitle = new Label();
		partyTitle.Text = UiLabels.Party.GetDescription();
		AddChild(partyTitle);
		var rightLocation = windowSizeCenter / 2 * 3 - partyTitle.Size.X / 2;
		partyTitle.Position = new Vector2(rightLocation, 120f);
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