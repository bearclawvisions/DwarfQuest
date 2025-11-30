namespace DwarfQuest.Business.Implementation;

public class OverworldService
{
    public bool IsInOverworld => true;
    public bool IsInSafeZone => false;
    public bool IsInCombat => false;
    public bool IsInMenu => false;
    
    public void Initialize()
    {
        // something like getting the needed location for loading scenes
    }
}