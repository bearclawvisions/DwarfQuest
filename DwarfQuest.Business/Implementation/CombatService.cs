using DwarfQuest.Data.Dto;
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
        // todo in this service already update player data and inventory
        
        _battleResult = new BattleResult
        {
            Experience = 66,
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
            Experience = 66,
            Money = 99,
            Message = "Congratulations!"
        };
    }

    public List<PlayerBattleResultInfo> GetPlayerPartyForBattleResults()
    {
        var players = new List<PlayerBattleResultInfo>();
        
        var data = _jsonService.Characters;
        var expToLevel = _jsonService.ExperienceToLevel;
        foreach (var player in data.Characters)
        {
            var playerLevel = player.Stats.Level;
            var expNeededToLevel = expToLevel.ExperienceToLevel.Find(x => x.Level == playerLevel);
            
            if (expNeededToLevel == null) 
                throw new Exception("Could not find experience needed to level for player");
            
            var info = new PlayerBattleResultInfo()
            {
                Name = player.Name,
                Experience = player.Stats.Experience,
                ExperienceToNextLevel = expNeededToLevel.Experience,
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