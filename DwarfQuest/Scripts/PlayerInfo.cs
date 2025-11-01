using DwarfQuest.Bridge.Extensions;
using DwarfQuest.Components.Container;
using DwarfQuest.Data.Dto;
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
	}

	public void InitializePlayerInfo(List<CombatDto> combatants)
	{
		foreach (var combatant in combatants)
		{
			var info = CombatPlayerInfo.Create(combatant);
			AddChild(info);
			_playerInfo.Add(info);
		}
	}

	public void UpdateHealth(CombatDto combatant)
	{
		var target = _playerInfo.FirstOrDefault(x => x.Id == combatant.Id);
		target?.UpdateHealth(combatant.Health, combatant.MaxHealth);
	}
}