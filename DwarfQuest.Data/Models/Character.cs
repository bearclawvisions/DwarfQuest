using System.Text.Json.Serialization;

namespace DwarfQuest.Data.Models;

public class Character
{
    [JsonPropertyName("id")]
    public int Id { get; set; }

    [JsonPropertyName("name")]
    public string Name { get; set; } = string.Empty;

    [JsonPropertyName("family")]
    public string Family { get; set; } = string.Empty;

    [JsonPropertyName("race")]
    public string Race { get; set; } = string.Empty;

    [JsonPropertyName("class")]
    public string Class { get; set; } = string.Empty;

    [JsonPropertyName("stats")]
    public CharacterStats Stats { get; set; } = new();
    
    [JsonPropertyName("formation")]
    public CharacterFormation Formation { get; set; }
}