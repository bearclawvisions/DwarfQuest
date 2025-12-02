using System.Numerics;

namespace DwarfQuest.Business.Implementation;

public class OverworldService
{
    private bool _isInOverworld = true;
    private bool _isInSafeZone = false;
    private bool _isInCombat = false;
    private bool _isInMenu = false;
    
    private Vector2 _playerPosition = new Vector2(0, 0);

    public Vector2 GetPlayerPosition()
    {
        // todo calculate needed position
        // todo save position on combat
        _playerPosition = new Vector2(392, 248);
        
        return _playerPosition;
    }
}