using DwarfQuest.Business.Interfaces;
using System.Numerics;

namespace DwarfQuest.Business.Implementation;

public class OverworldService
{
    private IOverworldEventListener _listener;
    
    // These are set by initialize
    private bool _isInOverworld;
    private bool _isInSafeZone;
    private bool _isInCombat; // use this to remember position, maybe boolean for scene change?
    private bool _isInMenu;

    private byte _stepsTaken;
    private int _totalSteps;
    private byte _encounterRate;
    private byte _zoneModifier; // could be use to tweak encounter rate, also reduce chance of encounters for low level areas/players

    private const byte EncounterRateMax = 100; // 100% chance of encounter
    private const byte StepCounterMax = 10; // increase encounter rate every 10 steps
    private const byte MultiplierMin = 5;
    private const byte MultiplierMax = 10;
    
    private Vector2 _playerPosition = new Vector2(0, 0);
    private Random _random = new();

    public void Initialize(IOverworldEventListener listener)
    {
        _listener = listener;
        GoToOverworld();
        _zoneModifier = 2;
    }

    public Vector2 GetPlayerPosition()
    {
        if (!_isInOverworld) 
            throw new Exception("Player is not in the overworld");
        
        // todo calculate needed position
        // todo save position on combat
        // _playerPosition = new Vector2(392, 248);
        _playerPosition = new Vector2(49, 339);
        
        return _playerPosition;
    }
    
    public void GoToCombat()
    {
        _isInOverworld = false;
        _isInCombat = true;
        ResetInternalSteps();
    }

    private void GoToMenu()
    {
        _isInOverworld = false;
        _isInMenu = true;
    }
    
    public void GoToSafeZone()
    {
        _isInOverworld = true;
        _isInSafeZone = true;
        ResetInternalSteps(); // maybe not?
    }
    
    private void GoToOverworld()
    {
        _isInOverworld = true;
        _isInCombat = false;
        _isInSafeZone = false;
        _isInMenu = false;
        
        _stepsTaken = 0;
        _encounterRate = 1;
    }

    // This is called every frame when waling
    public bool ShouldEncounter(int stepsTaken)
    {
        if (_isInSafeZone || _isInMenu)
            return false;
        
        // Godot frontend only passes through total steps, backend calculates actual steps since last call
        var actualSteps = stepsTaken - _totalSteps;
        _stepsTaken = (byte)actualSteps;
        
        CalculateEncounterRate();
        
        return _encounterRate == EncounterRateMax;
    }

    private void CalculateEncounterRate()
    {
        if (_isInSafeZone || _isInMenu) // probably redundant
            return;
        
        // calc encounter on steps taken?
        // more steps = higher chance of encounter
        // send steps from godot to backend?
        if (_stepsTaken <= StepCounterMax) 
            return;
        
        var multiplier = _random.Next(MultiplierMin, MultiplierMax);
        var calculatedRate = (byte)(_zoneModifier * multiplier);
        var newRate = (byte)(calculatedRate + _encounterRate);
        _encounterRate = newRate > EncounterRateMax ? EncounterRateMax : newRate;
        ResetInternalSteps();

        _listener.ShowMessageAsync($"Encounter rate: {_encounterRate}%");

        // different zone modifiers per area and take into account player level? (saved in the database)
        
        // based on a list, with chances for specific encounters, like rare encounter
        
        // let BattleManager choose enemies
    }
    
    private void ResetInternalSteps()
    {
        _totalSteps += _stepsTaken;
        _stepsTaken = 0;
    }
}