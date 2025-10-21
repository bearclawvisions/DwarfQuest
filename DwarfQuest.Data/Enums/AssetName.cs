using DwarfQuest.Data.Attributes;

namespace DwarfQuest.Data.Enums;

public enum AssetName
{
    [Category(AssetCategory.Theme), Path("combat_theme.tres")] CombatTheme,
    
    [Category(AssetCategory.Player), Path("placeholder.png")] PlayerPlaceholder,
    
    [Category(AssetCategory.Enemy), Path("placeholder.png")] EnemyPlaceholder,
}