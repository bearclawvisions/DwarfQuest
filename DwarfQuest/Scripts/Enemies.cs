using DwarfQuest.Bridge.Managers;
using DwarfQuest.Business.Implementation;
using DwarfQuest.Components.Character;
using DwarfQuest.Components.Container;
using DwarfQuest.Data.Enums;
using Godot;

namespace DwarfQuest.Scripts;

public partial class Enemies : CombatContainerBase
{
    private readonly CombatService _combatService = new();
    
    public override void _Ready()
    {
        InitializeParty();

        foreach (var participant in Participants)
        {
            participant.EnterCombat();
        }
    }
    
    private void InitializeParty()
    {
        var party = _combatService.GetEnemyCombatants();
        var texture = ResourceManager.GetAsset<Texture2D>(AssetName.EnemyPlaceholder); // move to json
        
        foreach (var memberInfo in party)
        {
            var character = new Combatant();
            character.CombatInfo = memberInfo;
            character.Name = memberInfo.Name;
            character.Position = new Vector2(memberInfo.CombatPosition.X, memberInfo.CombatPosition.Y);
            character.SetTexture(texture);
            AddChild(character);
            Participants.Add(character);
        }
    }
    
    public void RemoveEnemy(Combatant enemy)
    {
        if (!Participants.Contains(enemy)) 
            return;
        
        Participants.Remove(enemy);
    }
}