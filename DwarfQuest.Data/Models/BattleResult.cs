namespace DwarfQuest.Data.Models;

public class BattleResult
{
    public int Experience { get; set; }
    public int SkillPoints { get; set; }
    public int Money { get; set; }
    public string Message { get; set; } = string.Empty;
    // todo Items
}