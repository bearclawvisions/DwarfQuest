namespace DwarfQuest.Business.Interfaces;

public interface ICombatEventListener
{
    Task PlayAttackAnimationAsync();
    Task ShowMessageAsync(string message);
}