using DwarfQuest.Data.Dto;
using System.Text.Json.Serialization;

namespace DwarfQuest.Data.Models;

public class CharacterFormation
{
    public PositionVector Position { get; set; } = new();
}