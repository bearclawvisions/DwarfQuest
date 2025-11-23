using System.Text.Json.Serialization;

namespace DwarfQuest.Data.Models;

public class Stats
{
    public int Level { get; set; }
    public int Experience { get; set; }
    public int SkillPoints { get; set; }
    public int Health { get; set; }
    public int MaxHealth { get; set; }
    public int Mana { get; set; }
    public int Attack { get; set; }
    public int Defense { get; set; }
    public int Speed { get; set; }
    public int Strength { get; set; }
    public int Dexterity { get; set; }
    public int Constitution { get; set; }
    public int Intelligence { get; set; }
    public int Wisdom { get; set; }
    public int Charisma { get; set; }
    public int PhysicalStamina { get; set; }
    public int MentalStamina { get; set; }
}