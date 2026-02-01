using Microsoft.Data.Sqlite;

namespace DwarfQuest.Business.Implementation;

public class RepositoryBase
{
    private const string DwarfQuestDataDatabase = "DwarfQuest.Data/Database";
    private const string DwarfQuestDb = "template.db"; // todo: use the one from res:// or user://
    private readonly SqliteConnection _connection;
    
    public RepositoryBase()
    {
        var connectionString = InitializeConnectionString();
        _connection = new SqliteConnection(connectionString);
    }
    
    private static string InitializeConnectionString()
    {
        var solutionRoot = FindSolutionRoot();
        var dbPath = Path.Combine(solutionRoot, DwarfQuestDataDatabase, DwarfQuestDb);
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
    
    /// <summary>
    /// Method for executing SQL statements without a return value.
    /// </summary>
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
            throw new Exception($"Error executing SQL: {sql}", e);
        }
    }
    
    /// <summary>
    /// Read SQL statements that return a list of strings.
    /// </summary>
    private List<string> ReadSql(string sql)
    {
        var result = new List<string>();
        
        try
        {
            using var command = new SqliteCommand(sql, _connection);
            _connection.Open();
            using var reader = command.ExecuteReader();
            
            if (!reader.HasRows)
                return result;
            
            while (reader.Read())
            {
                result.Add(reader.GetString(0));
            }

            // closes automatically, because of using statement
        }
        catch (Exception e)
        {
            throw new Exception($"Error reading SQL: {sql}", e);
        }
        
        return result;
    }
    
    protected T ReadSingle<T>(string sql)
    {
        var result = ReadSql(sql);
        return result.Count == 0 ? default : (T)Convert.ChangeType(result[0], typeof(T));
    }
    
    protected T ReadAll<T>(string sql)
    {
        var result = ReadSql(sql);
        
        if (result.Count > 0) 
            return (T)Convert.ChangeType(result, typeof(T));
        
        throw new Exception($"No results found for query: {sql}");
    }
    
    protected void Execute(string sql)
    {
        ExecuteSql(sql);
    }
}