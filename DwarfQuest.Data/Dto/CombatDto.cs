using System.Numerics;

namespace DwarfQuest.Data.Dto;

public class CombatDto
{
    public int Id { get; set; } // todo expand this to replace position checks
    public required string Name { get; set; }
    public int Speed { get; set; }
    public int Health { get; set; }
    public int MaxHealth { get; set; }
    public int Damage { get; set; }
    public bool IsPlayer { get; set; }
    public bool IsSelected { get; set; }
    public bool IsDead { get; set; }
    public bool HasDebuff { get; set; }
    public int DebuffDuration { get; set; }
    public bool HasBuff { get; set; }
    public int BuffDuration { get; set; }
    public int Round { get; set; } = 0;
    public Vector2 CombatPosition { get; set; }
}