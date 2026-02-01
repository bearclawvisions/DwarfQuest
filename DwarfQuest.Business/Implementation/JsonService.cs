using DwarfQuest.Data.Models;
using System.Text.Json;

namespace DwarfQuest.Business.Implementation;

public class JsonService
{
    public CharacterCollection Characters => _characters;
    public MonsterCollection Monsters => _monsters;
    public ExperienceNeeded ExperienceToLevel => _expToLevel;
    
    private readonly CharacterCollection _characters;
    private readonly MonsterCollection _monsters;
    private readonly ExperienceNeeded _expToLevel;
    
    private const string DwarfQuestData = "DwarfQuest.Data";
    
    public JsonService()
    {
        _characters = LoadCharacterCollection();
        _monsters = LoadMonsterCollection();
        _expToLevel = LoadExperienceToLevelData();
    }

    private ExperienceNeeded LoadExperienceToLevelData()
    {
        var jsonPath = GetFilePath("Experience");
        var json = File.ReadAllText(jsonPath);
        var data = JsonSerializer.Deserialize<ExperienceNeeded>(json);
        
        if (data == null)
            throw new Exception("Could not load experience to level data");
        
        return data;
    }
    
    private CharacterCollection LoadCharacterCollection()
    {
        var jsonPath = GetFilePath("Characters");
        var json = File.ReadAllText(jsonPath);
        var data = JsonSerializer.Deserialize<CharacterCollection>(json);
        
        if (data == null)
            throw new Exception("Could not load characters");
        
        return data;
    }
    
    private MonsterCollection LoadMonsterCollection()
    {
        var jsonPath = GetFilePath("Enemies");
        var json = File.ReadAllText(jsonPath);
        var data = JsonSerializer.Deserialize<MonsterCollection>(json);
        
        if (data == null)
            throw new Exception("Could not load monsters");
        
        return data;
    }
    
    // todo move to utility class
    private static string GetFilePath(string filename)
    {
        var solutionRoot = FindSolutionRoot();
        return Path.Combine(solutionRoot, DwarfQuestData, "Json", $"{filename}.json");
    }
    
    private static string FindSolutionRoot()
    {
        var directory = new DirectoryInfo(AppContext.BaseDirectory);
        while (directory != null && directory.GetFiles("*.sln").Length == 0)
        {
            directory = directory.Parent;
        }
        return directory?.FullName ?? throw new DirectoryNotFoundException("Solution root not found");
    }
}