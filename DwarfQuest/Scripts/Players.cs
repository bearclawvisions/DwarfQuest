using DwarfQuest.Components.Character;
using DwarfQuest.Components.Container;
using System.Linq;

namespace DwarfQuest.Scripts;

public partial class Players : CombatContainerBase
{
    public override void _Ready()
    {
        Participants = GetNode("%Players").GetChildren().OfType<CharacterBase>().ToList();
    }
}