using DwarfQuest.Data.Dto;
using System.Text.Json.Serialization;

namespace DwarfQuest.Data.Models;

public class Formation
{
    public PositionVector Position { get; set; } = new();
}