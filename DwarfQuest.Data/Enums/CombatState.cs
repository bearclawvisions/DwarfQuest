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
    EndOfRound,
    
    // menu can be active
    AwaitingPlayerInput,
}