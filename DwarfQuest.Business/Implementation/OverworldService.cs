using System.Diagnostics;
using System.Numerics;

namespace DwarfQuest.Business.Implementation;

public class OverworldService
{
    // These are set by initialize
    private bool _isInOverworld;
    private bool _isInSafeZone;
    private bool _isInCombat;
    private bool _isInMenu;
    
    private int _stepsTaken;
    private int _totalSteps;
    private byte _encounterRate;
    private byte _gracePeriod;

    private const byte EncounterRateMax = 100;
    private const byte StepCounterMax = 255;
    private const byte GracePeriodMax = 10;
    
    private Vector2 _playerPosition = new Vector2(0, 0);
    private Stopwatch _timer;

    public void Initialize()
    {
        GoToOverworld();
        _timer = Stopwatch.StartNew();
        var test = _timer.ElapsedMilliseconds;
        _timer.Restart();
    }

    public Vector2 GetPlayerPosition()
    {
        if (!_isInOverworld) 
            throw new Exception("Player is not in the overworld");
        
        // todo calculate needed position
        // todo save position on combat
        _playerPosition = new Vector2(392, 248);
        
        return _playerPosition;
    }
    
    public void GoToCombat()
    {
        _isInOverworld = false;
        _isInCombat = true;
        ResetSteps();
    }

    public void GoToMenu()
    {
        _isInOverworld = true;
        _isInMenu = true;
        _isInCombat = false;
    }
    
    public void GoToSafeZone()
    {
        _isInOverworld = true;
        _isInSafeZone = true;
        ResetSteps(); // maybe not?
    }
    
    private void GoToOverworld()
    {
        if (!_isInMenu)
            _gracePeriod = GracePeriodMax;
        
        _isInOverworld = true;
        _isInCombat = false;
        _isInSafeZone = false;
        _isInMenu = false;
        
        _stepsTaken = 0;
        _encounterRate = 0;
    }

    public bool ShouldEncounter(int stepsTaken)
    {
        _stepsTaken += stepsTaken;
        
        if (_isInSafeZone || _isInMenu)
            return false;
        
        EncounterRate();
        
        return _encounterRate == EncounterRateMax;
    }

    private void EncounterRate()
    {
        if (_isInSafeZone || _isInMenu)
            return;
        
        // calc encounter on steps taken?
        // more steps = higher chance of encounter
        // send steps from godot to backend?
        if (_stepsTaken > StepCounterMax)
        {
            _encounterRate++;
            ResetSteps();
        }
        
        // time based encounter?
        // scene change in godot, more edge cases to cover?
        
        // hybrid approach? time and steps
        
        // maybe a different encounter rate for each zone? (saved in the database)
        
        // based on a list, with chances for specific encounters, like rare encounter
        
        // let BattleManager choose enemies
        
        // integrate grace periods to prevent too much encounters
        // lessen grace period when item is used or options set differently
    }
    
    private void ResetSteps()
    {
        _totalSteps += _stepsTaken;
        _stepsTaken = 0;
    }
}