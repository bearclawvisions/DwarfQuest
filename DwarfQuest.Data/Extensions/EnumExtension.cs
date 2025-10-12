using DwarfQuest.Data.Attributes;
using DwarfQuest.Data.Enums;
using System.Reflection;

namespace DwarfQuest.Data.Extensions;

public static class EnumExtension
{
    public static string GetPath(this Enum enumValue)
    {
        var type = enumValue.GetType();
        var name = Enum.GetName(type, enumValue);
        
        if (name == null) return "No enum name found";
        
        var field = type.GetField(name);
        var attribute = field?.GetCustomAttribute<PathAttribute>();
        
        return attribute?.Path ?? "No path attribute found";
    }
    
    public static AssetCategory GetCategory(this Enum enumValue)
    {
        var type = enumValue.GetType();
        var name = Enum.GetName(type, enumValue);
        
        if (name == null) return AssetCategory.Null;
        
        var field = type.GetField(name);
        var attribute = field?.GetCustomAttribute<CategoryAttribute>();
        
        return attribute?.Category ?? AssetCategory.Null;
    }
}