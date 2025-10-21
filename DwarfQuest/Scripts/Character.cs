using DwarfQuest.Business.Implementation;
using DwarfQuest.Components.Character;

namespace DwarfQuest.Scripts;

public partial class Character : CharacterBase
{
    private readonly CombatService _combatService = new CombatService();
    
    public override void _Ready()
    {
        Damage = 55;
        IsPlayer = true;
        EnterCombat();
    }

    public override void OnDeath()
    {
        base.OnDeath();
        // todo no death or revive in combat, but select replacement
    }
}