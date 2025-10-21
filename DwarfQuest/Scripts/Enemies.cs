using System.Linq;
using DwarfQuest.Components.Character;
using DwarfQuest.Components.Container;

namespace DwarfQuest.Scripts;

public partial class Enemies : CombatContainerBase
{
    public override void _Ready()
    {
        Participants = GetNode("%Enemies").GetChildren().OfType<Combatant>().ToList();
    }
    
    public void RemoveEnemy(Combatant enemy)
    {
        if (!Participants.Contains(enemy)) 
            return;
        
        Participants.Remove(enemy);
    }
}