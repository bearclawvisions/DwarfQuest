using DwarfQuest.Data.Dto;
using DwarfQuest.Data.Models;
using System.Numerics;

namespace DwarfQuest.Business.Implementation;

public class CombatService
{
    private readonly JsonService _jsonService = new();
    private BattleResult _battleResult = new();

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
                Id = combatant.Id,
                Name = combatant.Name,
                Health = stats.Health,
                MaxHealth = stats.MaxHealth,
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
                MaxHealth = stats.MaxHealth,
                Damage = stats.Attack,
                Speed = stats.Speed,
                IsPlayer = false,
                CombatPosition = new Vector2(formation.X, formation.Y)
            };
            
            combatants.Add(dto);
        }
        
        return combatants;
    }

    public BattleResult GetBattleResult()
    {
        return _battleResult;
    }

    public void SetBattleResult(List<CombatDto> defeated)
    {
        // todo do magic to set stuff
        
        _battleResult = new BattleResult
        {
            Experience = 100,
            Money = 99,
            Message = "Congratulations!"
        };
    }

    public void ClearBattleResult()
    {
        _battleResult = new BattleResult();
    }
}