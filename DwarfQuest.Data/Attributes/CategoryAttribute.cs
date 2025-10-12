using DwarfQuest.Data.Enums;

namespace DwarfQuest.Data.Attributes;

[AttributeUsage(AttributeTargets.Field)]
public class CategoryAttribute(AssetCategory category) : Attribute
{
    public AssetCategory Category { get; } = category;
}