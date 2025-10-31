using DwarfQuest.Data.Dto;
using System.Numerics;

namespace DwarfQuest.Business.Implementation;

public class CombatService
{
    private readonly JsonService _jsonService = new();

    public List<CombatDto> GetPlayerCombatants()
    {
        var combatants = new List<CombatDto>();
        
        var data = _jsonService.Characters;
        foreach (var combatant in data.Characters)
        {
            var stats = combatant.Stats;
            var formation = combatant.Formation.Position;
            var dto = new CombatDto
            {
                Name = combatant.Name,
                Health = stats.Health,
                Damage = stats.Attack,
                Speed = stats.Speed,
                IsPlayer = true,
                CombatPosition = new Vector2(formation.X, formation.Y)
            };
            
            combatants.Add(dto);
        }
        
        return combatants;
    }
    
    public List<CombatDto> GetEnemyCombatants()
    {
        var combatants = new List<CombatDto>();
        
        var data = _jsonService.Monsters;
        foreach (var combatant in data.Monsters)
        {
            var stats = combatant.Stats;
            var formation = combatant.Formation.Position;
            var dto = new CombatDto
            {
                Name = combatant.Name,
                Health = stats.Health,
                Damage = stats.Attack,
                Speed = stats.Speed,
                IsPlayer = false,
                CombatPosition = new Vector2(formation.X, formation.Y)
            };
            
            combatants.Add(dto);
        }
        
        return combatants;
    }
}