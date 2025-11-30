using DwarfQuest.Bridge.Extensions;
using DwarfQuest.Bridge.Managers;
using DwarfQuest.Components.Container;
using DwarfQuest.Data.Enums;
using DwarfQuest.Data.Models;
using Godot;
using System.Collections.Generic;
using System.Linq;

namespace DwarfQuest.Scripts;

public partial class PlayerInfo : VBoxContainer
{
    private readonly List<CombatPlayerInfo> _playerInfo = [];
	
	public override void _Ready()
	{
        this.ClearPlaceholders();
        const int padding = 10;
        Theme = ResourceManager.GetAsset<Theme>(AssetName.CombatTheme);
        AnchorsPreset = (int)LayoutPreset.BottomRight;
        Position = new Vector2(Position.X - padding, Position.Y - padding);
	}

	public void InitializePlayerInfo(List<CombatantInfo> combatants)
	{
		foreach (var combatant in combatants)
		{
			var info = CombatPlayerInfo.Create(combatant);
			AddChild(info);
			_playerInfo.Add(info);
		}
	}

	public void UpdateHealth(CombatantInfo combatant)
	{
		var target = _playerInfo.FirstOrDefault(x => x.Id == combatant.Id);
		target?.UpdateHealth(combatant.Health, combatant.MaxHealth);
	}
}