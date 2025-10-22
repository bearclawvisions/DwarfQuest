using DwarfQuest.Bridge.Managers;
using DwarfQuest.Components.Character;
using DwarfQuest.Components.Container;
using DwarfQuest.Data.Dto;
using DwarfQuest.Data.Enums;
using Godot;
using System.Collections.Generic;

namespace DwarfQuest.Scripts;

public partial class Enemies : CombatContainerBase
{
    public void InitializeParty(List<CombatDto> combatants)
    {
        var texture = ResourceManager.GetAsset<Texture2D>(AssetName.EnemyPlaceholder); // move to json
        
        foreach (var memberInfo in combatants)
        {
            var character = new Combatant();
            character.CombatInfo = memberInfo;
            character.Name = memberInfo.Name;
            character.Position = new Vector2(memberInfo.CombatPosition.X, memberInfo.CombatPosition.Y);
            character.SetTexture(texture);
            AddChild(character);
            Participants.Add(character);
            character.EnterCombat();
        }
    }
    
    public void RemoveEnemy(CombatDto enemy)
    {
        var combatant = Participants.Find(x => x.CombatInfo.CombatPosition == enemy.CombatPosition);
        Participants.Remove(combatant);
    }
}