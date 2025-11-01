using DwarfQuest.Data.Dto;

namespace DwarfQuest.Business.Interfaces;

public interface ICombatEventListener
{
    Task ShowMessageAsync(string message);
    Task CombatantDeathAsync(CombatDto combatant);
    Task PlayAttackAnimationAsync();
    void UpdatePlayerInfo(CombatDto combatant);
}