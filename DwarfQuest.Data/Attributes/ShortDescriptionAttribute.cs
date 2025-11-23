namespace DwarfQuest.Data.Attributes;

[AttributeUsage(AttributeTargets.Field)]
public class ShortDescriptionAttribute(string description) : Attribute
{
    public string ShortDescription { get; } = description;
}