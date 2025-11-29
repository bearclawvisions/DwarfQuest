namespace DwarfQuest.Data.Models;

public class ExperienceNeeded
{
    public List<ExpNeeded> ExperienceToLevel { get; set; } = new();
}

// this should be a table and save current exp needed in character table
public class ExpNeeded
{
    public int Level { get; set; }
    public int Experience { get; set; }
}