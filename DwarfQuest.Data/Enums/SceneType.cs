using DwarfQuest.Data.Attributes;

namespace DwarfQuest.Data.Enums;

public enum SceneType
{
    [Path("combat")] Combat,
    [Path("combat_results")] CombatResults,
    [Path("overworld")] Overworld,
    [Path("overworld_player")] OverworldPlayer,
}