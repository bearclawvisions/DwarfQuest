using System.Text.Json.Serialization;

namespace DwarfQuest.Data.Models;

public class Character
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Family { get; set; } = string.Empty;
    public string Race { get; set; } = string.Empty;
    public string Class { get; set; } = string.Empty;
    public CharacterStats Stats { get; set; } = new();
    public CharacterFormation Formation { get; set; } = new();
}