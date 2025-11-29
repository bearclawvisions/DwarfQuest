using DwarfQuest.Data.Attributes;
using System.ComponentModel;

namespace DwarfQuest.Data.Enums;

public enum UiLabels
{
    [Description("XP")]
    Experience,
    
    [Description("SP")]
    SkillPoints,
    
    [Description("Money")]
    Money,
    
    [Description("Items")]
    Items,
    
    [Description("Party")]
    Party,
    
    [Description("Battle Results")]
    BattleResults,
    
    [Description(" - ")]
    SpacedDash,
}