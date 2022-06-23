using System.Text.RegularExpressions;
using Dapper;
using MySql.Data.MySqlClient;

namespace DapperDynamic;

public class DatabaseManager
{
    private static DatabaseManager? _instance;
    private MySqlConnection _connection;
    
    private DatabaseManager(string host, string port, string user, string password, string database)
    {
        _connection = new MySqlConnection($"server={host};port={port};user={user};password={password};database={database}");
        _connection.Open();
    }
    
    public static DatabaseManager? Instance => _instance;
    
    public static void Initialize(string user="root", string password="", string database="example", string host="localhost", string port="3306")
    {
        _instance = new DatabaseManager(host, port, user, password, database);
    }

    public bool CreateTable(string name)
    {
        if(name.Length > 64) throw new ArgumentException("Table name cannot be longer than 64 characters");
        Regex regex = new Regex(@"^[a-zA-Z0-9_\-]{1,32}$");
        if(!regex.IsMatch(name)) throw new ArgumentException("Table name cannot contain special characters");
        int result;
        using (var transaction = _connection.BeginTransaction())
        {
            result = _connection.Execute("INSERT INTO `usertables` (`tablename`) VALUES (@name)", new { name });
            _connection.Execute($"CREATE TABLE {name} (id INT NOT NULL AUTO_INCREMENT PRIMARY KEY)");
            transaction.Commit();
        }
        return result == 1;
    }
    
    public bool DeleteTable(string name)
    {
        if(name.Length > 64) throw new ArgumentException("Table name cannot be longer than 64 characters");
        Regex regex = new Regex(@"^[a-zA-Z0-9_\-]{1,32}$");
        if(!regex.IsMatch(name)) throw new ArgumentException("Table name cannot contain special characters");
        int result;
        using (var transaction = _connection.BeginTransaction())
        {
            _connection.Execute("DELETE FROM `usertablescolumns` WHERE `tablename` = @name", new { name });
            result = _connection.Execute("DELETE FROM `usertables` WHERE `tablename` = @name", new { name });
            _connection.Execute($"DROP TABLE {name}");
            transaction.Commit();
        }
        return result == 1;
    }
}