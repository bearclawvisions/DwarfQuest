using System.Numerics;

namespace DwarfQuest.Business.Implementation;

public class OverworldService
{
    private bool _isInOverworld = true;
    private bool _isInSafeZone = false;
    private bool _isInCombat = false;
    private bool _isInMenu = false;
    
    private int _stepsTaken = 0;
    private int _encounterRate = 0;
    private int _gracePeriod = 0;
    
    private Vector2 _playerPosition = new Vector2(0, 0);

    public Vector2 GetPlayerPosition()
    {
        if (!_isInOverworld) 
            throw new Exception("Player is not in the overworld");
        
        // todo calculate needed position
        // todo save position on combat
        _playerPosition = new Vector2(392, 248);
        
        return _playerPosition;
    }
    
    public bool GoToCombat()
    {
        _isInOverworld = false;
        _isInCombat = true;
        
        return _isInCombat;
    }
    
    public bool GoToMenu()
    {
        _isInOverworld = true;
        _isInMenu = true;
        
        return _isInMenu;
    }
    
    public bool GoToSafeZone()
    {
        _isInOverworld = true;
        _isInSafeZone = true;
        
        return _isInSafeZone;
    }
    
    public bool GoToOverworld()
    {
        _isInOverworld = true;
        _isInCombat = false;
        _isInSafeZone = false;

        _gracePeriod = 1;
        
        return _isInOverworld;
    }

    public bool ShouldEncounter()
    {
        if (_isInSafeZone || _isInMenu)
            return false;
        
        EncounterRate();
        
        return _encounterRate == 1;
    }

    private void EncounterRate()
    {
        if (_isInSafeZone || _isInMenu) return;
        
        // calc encounter on steps taken?
        // more steps = higher chance of encounter
        // send steps from godot to backend?
        
        // time based encounter?
        // scene change in godot, more edge cases to cover?
        
        // hybrid approach? time and steps
        
        // take into account the safe zone
        // maybe a different encounter rate for each zone? (saved in the database)
        
        // based on a list, with chances for specific encounters, like rare encounter
        
        // let BattleManager choose enemies
        
        // integrate grace periods to prevent too much encounters
        // lessen grace period when item is used or options set differently
    }
}