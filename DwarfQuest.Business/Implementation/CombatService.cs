using DwarfQuest.Data.Models;
using System.Numerics;

namespace DwarfQuest.Business.Implementation;

public class CombatService
{
    private readonly JsonService _jsonService = new();
    private BattleResult _battleResult = new();

    public List<CombatantInfo> GetPlayerCombatants()
    {
        var combatants = new List<CombatantInfo>();
        
        var data = _jsonService.Characters;
        foreach (var combatant in data.Characters)
        {
            var stats = combatant.Stats;
            var formation = combatant.Formation.Position;
            var dto = new CombatantInfo
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
    
    public List<CombatantInfo> GetEnemyCombatants()
    {
        var combatants = new List<CombatantInfo>();
        
        var data = _jsonService.Monsters;
        foreach (var combatant in data.Monsters)
        {
            var stats = combatant.Stats;
            var formation = combatant.Formation.Position;
            var dto = new CombatantInfo
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
        _battleResult = new BattleResult
        {
            Experience = 100,
            SkillPoints = 5,
            Money = 99,
            Items =
            [
                new Item { Name = "Potion of Healing", Amount = 3, },
                new Item { Name = "Herbs", Amount = 5, }
            ],
            Message = "Congratulations!"
        };
        
        return _battleResult;
    }

    public void SetBattleResult(List<CombatantInfo> defeated)
    {
        // todo do magic to set stuff
        
        _battleResult = new BattleResult
        {
            Experience = 100,
            Money = 99,
            Message = "Congratulations!"
        };
    }

    public List<PlayerBattleResultInfo> GetPlayerPartyForBattleResults()
    {
        var players = new List<PlayerBattleResultInfo>();
        
        var data = _jsonService.Characters;
        foreach (var player in data.Characters)
        {
            var info = new PlayerBattleResultInfo()
            {
                Name = player.Name,
                Experience = player.Stats.Experience,
                SkillPoints = player.Stats.SkillPoints,
            };
            
            players.Add(info);
        }
        
        return players;
    }

    public void ClearBattleResult()
    {
        _battleResult = new BattleResult();
    }
}