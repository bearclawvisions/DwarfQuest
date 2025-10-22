using DwarfQuest.Bridge.Managers;
using DwarfQuest.Business.Implementation;
using DwarfQuest.Components.Character;
using DwarfQuest.Components.Container;
using DwarfQuest.Data.Dto;
using DwarfQuest.Data.Enums;
using Godot;
using System.Collections.Generic;

namespace DwarfQuest.Scripts;

public partial class Players : CombatContainerBase
{
    private readonly CombatService _combatService = new();
    
    public override void _Ready()
    {
    }
    
    public void InitializeParty(List<CombatDto> combatants)
    {
        // var party = _combatService.GetPlayerCombatants();
        var texture = ResourceManager.GetAsset<Texture2D>(AssetName.PlayerPlaceholder); // move to json
        
        foreach (var memberInfo in combatants)
        {
            var character = new Combatant();
            character.CombatInfo = memberInfo;
            character.Name = memberInfo.Name;
            character.Position = new Vector2(memberInfo.CombatPosition.X, memberInfo.CombatPosition.Y);
            character.SetTexture(texture);
            AddChild(character);
            Participants.Add(character);
        }
        
        foreach (var participant in Participants)
        {
            participant.EnterCombat();
        }
    }
}