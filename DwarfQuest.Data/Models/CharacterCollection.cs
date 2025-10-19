using DwarfQuest.Data.Dto;
using System.Text.Json.Serialization;

namespace DwarfQuest.Data.Models;

public class CharacterCollection
{
    [JsonPropertyName("Characters")]
    public List<Character> Characters { get; set; } = new();
}
