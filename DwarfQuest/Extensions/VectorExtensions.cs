namespace DwarfQuest.Extensions;

public static class VectorExtensions
{
    public static Godot.Vector2 ToGodotVector(this System.Numerics.Vector2 v) => new(v.X, v.Y);

    public static System.Numerics.Vector2 ToNumericsVector(this Godot.Vector2 v) => new(v.X, v.Y);
}