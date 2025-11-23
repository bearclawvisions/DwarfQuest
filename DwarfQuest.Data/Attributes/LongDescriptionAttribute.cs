namespace DwarfQuest.Data.Attributes;

[AttributeUsage(AttributeTargets.Field)]
public class LongDescriptionAttribute(string description) : Attribute
{
    public string LongDescription { get; } = description;
}