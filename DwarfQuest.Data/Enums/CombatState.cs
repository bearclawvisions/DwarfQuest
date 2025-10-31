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
    TargetSelectionEnemy,
    TargetSelectionPlayer,
    EndOfRound,
    
    // menu can be active
    AwaitingPlayerInput,
}