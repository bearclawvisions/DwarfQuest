using DwarfQuest.Components.Character;
using DwarfQuest.Components.Container;
using System.Linq;

namespace DwarfQuest.Scripts;

public partial class Enemies : CombatContainerBase
{
    public override void _Ready()
    {
        Participants = GetNode("%Enemies").GetChildren().OfType<CharacterBase>().ToList();
    }
    
    public void RemoveEnemy(CharacterBase enemy)
    {
        if (!Participants.Contains(enemy)) 
            return;
        
        Participants.Remove(enemy);
    }
}