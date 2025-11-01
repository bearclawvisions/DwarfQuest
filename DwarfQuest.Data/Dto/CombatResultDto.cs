namespace DwarfQuest.Data.Dto;

public class CombatResultDto
{
    public int Experience { get; set; }
    public int Money { get; set; }
    public required string[] Items { get; set; } // should be a List<Items>
}