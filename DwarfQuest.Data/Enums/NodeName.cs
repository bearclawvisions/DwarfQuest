using DwarfQuest.Data.Attributes;

namespace DwarfQuest.Data.Enums;

public enum NodeName
{
    // Overworld
    [Path("%Ground")] Ground,
    [Path("%Walls")] Walls,
    
    [Path("Sprite")] Sprite,
    // [Path("Control/HealthBar")] HealthBar,
}