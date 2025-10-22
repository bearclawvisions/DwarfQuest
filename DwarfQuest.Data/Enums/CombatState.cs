namespace DwarfQuest.Data.Enums;

public enum CombatState
{
    EnterCombat,
    ExitCombat,
    NewTurn,
    PlayerTurn,
    EnemyTurn,
    Run,
    HandleAnimation,
    TargetSelection,
    
    // menu can be active
    AwaitingPlayerInput,
}