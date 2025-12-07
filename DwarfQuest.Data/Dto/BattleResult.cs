using DwarfQuest.Data.Models;

namespace DwarfQuest.Data.Dto;

public class BattleResult
{
    public int Experience { get; set; }
    public int SkillPoints { get; set; }
    public int Money { get; set; }
    public List<Item> Items { get; set; } = [];
    public string Message { get; set; } = string.Empty;
}