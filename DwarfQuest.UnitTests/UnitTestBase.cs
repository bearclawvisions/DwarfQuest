using DwarfQuest.Bridge.Managers;
using DwarfQuest.Business.Implementation;

namespace DwarfQuest.UnitTests;

public class UnitTestBase
{
    public CombatService _combatService;
    public GameService _gameService;
    public JsonService _jsonService;
    public OverworldService _overworldService;

    private string _connectionString;
    
    [SetUp]
    public void Setup()
    {
        // services
        _combatService = new CombatService();
        _gameService = new GameService();
        _jsonService = new JsonService();
        _overworldService = new OverworldService();
        
    }

    [TearDown]
    public void TearDown()
    {
        _combatService = null;
        _gameService = null;
        _jsonService = null;
        _overworldService = null;
    }
}