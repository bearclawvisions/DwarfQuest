namespace DwarfQuest.Data.Attributes;

[AttributeUsage(AttributeTargets.Field)]
public class PathAttribute(string path) : Attribute
{
    public string Path { get; } = path;
}