using DwarfQuest.Data.Dto;
using System.Text.Json.Serialization;

namespace DwarfQuest.Data.Models;

public class CharacterCollection
{
    public List<Character> Characters { get; init; } = new ();
}
