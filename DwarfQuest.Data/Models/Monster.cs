namespace DwarfQuest.Data.Models;

public class Monster
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Family { get; set; } = string.Empty;
    public Stats Stats { get; set; } = new();
    public Formation Formation { get; set; } = new();
}