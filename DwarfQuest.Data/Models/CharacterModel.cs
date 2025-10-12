namespace DwarfQuest.Data.Models;

public class CharacterModel
{
    public required string Name { get; set; }
    public int Speed { get; set; }
    public int Health { get; set; }
    public int Damage { get; set; }
    public bool IsPlayer { get; set; }
}