using DwarfQuest.Data.Models;

namespace DwarfQuest.Business.Interfaces;

public interface ICombatEventListener
{
    Task ShowMessageAsync(string message);
    Task CombatantDeathAsync(CombatantInfo combatant);
    Task PlayAttackAnimationAsync();
    void UpdatePlayerInfo(CombatantInfo combatant);
}