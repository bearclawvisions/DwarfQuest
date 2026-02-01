using DwarfQuest.Data.Models;
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
        const string sql = "INSERT INTO Characters (Name, Race, Class, Family) VALUES ('Test', 'Test', 'Test', 'Test')";
        ExecuteSql(sql);
    }

    [Test]
    public void ReadFromDatabase()
    {
        const string sql = "SELECT * FROM Characters";
        var result = ReadSql<Character>(sql);
        Console.WriteLine(result);
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
    
    private List<T> ReadSql<T>(string sql) where T : new()
    {
        var result = new List<T>();
        
        try
        {
            using var command = new SqliteCommand(sql, _connection);
            _connection.Open();
            using var reader = command.ExecuteReader();
            
            if (!reader.HasRows)
                return result;
            
            while (reader.Read())
            {
                var item = new T();
                var properties = typeof(T).GetProperties();
            
                for (int i = 0; i < reader.FieldCount; i++)
                {
                    var columnName = reader.GetName(i);
                    var property = properties.FirstOrDefault(p => p.Name.Equals(columnName, StringComparison.OrdinalIgnoreCase));

                    if (property == null || !property.CanWrite) continue;
                    
                    var value = reader.GetValue(i);
                    if (value != DBNull.Value)
                    {
                        property.SetValue(item, Convert.ChangeType(value, property.PropertyType));
                    }
                }
            
                result.Add(item);
            }
            // closes automatically, because of using statement
        }
        catch (Exception e)
        {
            throw new Exception($"Error reading SQL: {sql}", e);
        }
        
        return result;
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
        while (directory != null && directory.GetFiles("*.sln").Length == 0)
        {
            directory = directory.Parent;
        }
        return directory?.FullName ?? throw new DirectoryNotFoundException("Solution root not found");
    }
}