using DwarfQuest.Business.Implementation;

namespace DwarfQuest.Bridge.Managers;

public static class GameManager
{
    public static readonly CombatService CombatService = new();
    
    public static void Initialize()
    {
    }
}