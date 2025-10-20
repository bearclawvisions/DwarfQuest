using DwarfQuest.Bridge.Components.Character;

namespace DwarfQuest.Scripts;

public partial class Character : CharacterBase
{
    
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