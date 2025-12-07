using Microsoft.Data.Sqlite;

namespace DwarfQuest.UnitTests.Tools;

public class DatabaseTools : UnitTestBase
{
    private const string DwarfQuestData = "DwarfQuest.Data";
    private readonly SqliteConnection _connection;
    
    public DatabaseTools()
    {
        var connectionString = InitializeConnectionString();
        _connection = new SqliteConnection(connectionString);
    }
    
    [Test]
    public void TestDatabaseConnection()
    {
        try
        {
            _connection.Open();
            
            _connection.Close();
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
        }
    }

    [Test]
    public void CreateTables()
    {
        var slnRoot = FindSolutionRoot();
        var queryPath = Path.Combine(slnRoot, DwarfQuestData, "SqlQueries", "CreateTables.sql");
        var sql = File.ReadAllText(queryPath);

        ExecuteSql(sql);
        Console.WriteLine("Tables created successfully");
    }

    [Test]
    public void DropTables()
    {
        // Get all table names (excluding sqlite internal tables)
        const string getTablesQuery = """
                                      SELECT name FROM sqlite_master 
                                      WHERE type='table' 
                                      AND name NOT LIKE 'sqlite_%';
                                      """;
        
        var tables = new List<string>();
        
        using (var command = new SqliteCommand(getTablesQuery, _connection))
        {
            _connection.Open();
            using var reader = command.ExecuteReader();
            while (reader.Read())
            {
                tables.Add(reader.GetString(0));
            }
        }
        
        foreach (var table in tables)
        {
            var dropCommand = $"DROP TABLE IF EXISTS [{table}];";
            ExecuteSql(dropCommand);
        }
        
        Console.WriteLine($"Dropped {tables.Count} tables successfully");
    }

    private void ExecuteSql(string sql)
    {
        try
        {
            using var command = new SqliteCommand(sql, _connection);
            _connection.Open();
            command.ExecuteNonQuery();
            // closes automatically, because of using statement
        }
        catch (Exception e)
        {
            Console.WriteLine(e.Message);
        }
    }
    
    private static string InitializeConnectionString()
    {
        var solutionRoot = FindSolutionRoot();
        var dbPath = Path.Combine(solutionRoot, DwarfQuestData, "Database", "template.db");
        return $"Data Source={dbPath}";
    }
    
    private static string FindSolutionRoot()
    {
        var directory = new DirectoryInfo(AppContext.BaseDirectory);
        while (directory != null && !directory.GetFiles("*.sln").Any())
        {
            directory = directory.Parent;
        }
        return directory?.FullName ?? throw new DirectoryNotFoundException("Solution root not found");
    }
}