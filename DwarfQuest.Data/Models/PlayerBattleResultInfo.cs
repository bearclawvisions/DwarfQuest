namespace DwarfQuest.Data.Models;

public class PlayerBattleResultInfo
{
    public string Name { get; set; } = string.Empty;
    public int Experience { get; set; }
    public int ExperienceToNextLevel { get; set; }
    public int SkillPoints { get; set; }
}