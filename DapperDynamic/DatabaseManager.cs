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
    }
    
    public static DatabaseManager? Instance => _instance;

    public bool HandleInTransaction(Func<bool> statement, bool reopenConnection = true)
    {
        if(reopenConnection)
            try
            {
                _connection.Open();
            }
            catch(InvalidOperationException)
            {
                // ignored
            }

        using var transaction = _connection.BeginTransaction();
        try
        {
            bool result = statement.Invoke();
            if (result) transaction.Commit();
            else transaction.Rollback();
            return result;
        }
        catch (Exception)
        {
            transaction.Rollback();
            throw;
        }
        finally
        {
            _connection.Close();
        }
    }
    
    public static void Initialize(string user="root", string password="", string database="example", string host="localhost", string port="3306")
    {
        _instance = new DatabaseManager(host, port, user, password, database);
    }

    public bool CreateTable(string name, bool dropIfExists = false)
    {
        if(name.Length > 64) throw new ArgumentException("Table name cannot be longer than 64 characters");
        Regex regex = new Regex(@"^[a-zA-Z0-9_\-]{1,32}$");
        if(!regex.IsMatch(name)) throw new ArgumentException("Table name cannot contain special characters");
        if (_isTableExists(name)) DeleteTable(name);
        return HandleInTransaction(() =>
        {
            int result = _connection.Execute("INSERT INTO `usertables` (`tablename`) VALUES (@name)", new { name });
            _connection.Execute($"CREATE TABLE `{name}` (id INT NOT NULL AUTO_INCREMENT PRIMARY KEY)");
            return result == 1;
        });
    }
    
    public bool DeleteTable(string name)
    {
        if(name.Length > 64) throw new ArgumentException("Table name cannot be longer than 64 characters");
        Regex regex = new Regex(@"^[a-zA-Z0-9_\-]{1,32}$");
        if(!regex.IsMatch(name)) throw new ArgumentException("Table name cannot contain special characters");
        return HandleInTransaction(() =>
        {
            _connection.Execute("DELETE FROM `usertablescolumns` WHERE `tablename` = @name", new { name });
            int result = _connection.Execute("DELETE FROM `usertables` WHERE `tablename` = @name", new { name });
            _connection.Execute($"DROP TABLE `{name}`");
            return result == 1;
        });
    }

    private bool _isTableExists(string name)
    {
        return _connection.Query("SELECT * FROM information_schema.TABLES WHERE TABLE_NAME = @name", new { name })
            .FirstOrDefault() is not null;
    }

    public bool CreateColumn(string tablename, string colname, Type type, string color)
    {
        if(!_isTableExists(tablename)) throw new ArgumentException("Table does not exist");
        if(colname.Length > 64) throw new ArgumentException("Column name cannot be longer than 64 characters");
        Regex regex = new Regex(@"^[a-zA-Z0-9_\-]{1,32}$");
        if(!regex.IsMatch(colname)) throw new ArgumentException("Column name cannot contain special characters");

        string cstype, sqltype;
        if (type == typeof(bool))
        {
            cstype = "BOOL";
            sqltype = "TINYINT(1)";
        }
        else if (type == typeof(char))
        {
            cstype = "CHAR";
            sqltype = "CHAR(1)";
        }
        else if (type == typeof(decimal))
        {
            cstype = "DECIMAL";
            sqltype = "DECIMAL(10,4)";
        }
        else if (type == typeof(double))
        {
            cstype = "DOUBLE";
            sqltype = "DOUBLE";
        }
        else if (type == typeof(int))
        {
            cstype = "INT";
            sqltype = "INT";
        }
        else if (type == typeof(string))
        {
            cstype = "STRING";
            sqltype = "VARCHAR(255)";
        }
        else
        {
            throw new ArgumentException("Unsupported type");
        }

        return HandleInTransaction(() =>
        {
            int result = _connection.Execute("INSERT INTO `usertablescolumns`" +
                                             "(`tablename`, `colname`, `csharptype`, `color`)" +
                                             "VALUES (@tablename, @colname, @cstype, @color)",
                new { tablename, colname, cstype, color });
            _connection.Execute($"ALTER TABLE `{tablename}` ADD COLUMN `{colname}` {sqltype}");
            return result == 1;
        });
    }

    public bool DeleteColumn(string tablename, string colname)
    {
        if(!_isTableExists(tablename)) throw new ArgumentException("Table does not exist");
        if(colname.Length > 64) throw new ArgumentException("Column name cannot be longer than 64 characters");
        Regex regex = new Regex(@"^[a-zA-Z0-9_\-]{1,32}$");
        if(!regex.IsMatch(colname)) throw new ArgumentException("Column name cannot contain special characters");

        return HandleInTransaction(() =>
        {
            int result = _connection.Execute(
                "DELETE FROM `usertablescolumns` WHERE tablename=@tablename AND colname=@colname",
                new { tablename, colname});
            _connection.Execute($"ALTER TABLE `{tablename}` DROP COLUMN `{colname}`");
            return result == 1;
        });
    }
}