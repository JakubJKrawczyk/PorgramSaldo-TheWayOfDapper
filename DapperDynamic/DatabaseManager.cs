using System.Drawing;
using System.Globalization;
using Dapper;
using DapperDynamic.queries;
using DapperDynamic.structures;
using MySql.Data.MySqlClient;

namespace DapperDynamic;

public class DatabaseManager
{
    private static DatabaseManager? _instance;
    private MySqlConnection _connection;
    private static Dictionary<Type, Tuple<string, string>> _typeMap = new()
    {
        {typeof(bool), new Tuple<string, string>("BOOL", "TINYINT(1)")},
        {typeof(char), new Tuple<string, string>("CHAR", "CHAR(1)")},
        {typeof(decimal), new Tuple<string, string>("DECIMAL", "DECIMAL(10,4)")},
        {typeof(double), new Tuple<string, string>("DOUBLE", "DOUBLE")},
        {typeof(int), new Tuple<string, string>("INT", "INT")},
        {typeof(string), new Tuple<string, string>("STRING", "VARCHAR(255)")},
    };
    private static Dictionary<string, Type> _reverseTypeMap = new(){
        {"BOOL", typeof(bool)},
        {"CHAR", typeof(char)},
        {"DECIMAL", typeof(decimal)},
        {"DOUBLE", typeof(double)},
        {"INT", typeof(int)},
        {"STRING", typeof(string)},
    };
    
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
            catch (InvalidOperationException){}

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

    private static string _generateRealName(bool column = false)
    {
        return column ? $"col_{DateTime.Now.ToFileTime()}" : $"table_{DateTime.Now.ToFileTime()}";
    }

    public bool CreateTable(string name, bool dropIfExists = false)
    {
        if (_isTableExists(name))
        {
            if (!dropIfExists) return false;
            if (!DeleteTable(name)) throw new Exception("Failed to delete table");
        }
        string realName = _generateRealName();
        return HandleInTransaction(() =>
        {
            int result = _connection.Execute(
                "INSERT INTO `usertables` (`tablerealname`, `tabledisplayname`) VALUES (@realname, @displayname)", 
                new { displayname = name, realname = realName });
            _connection.Execute($"CREATE TABLE `{realName}` (id INT NOT NULL AUTO_INCREMENT PRIMARY KEY)");
            return result == 1;
        });
    }
    
    public bool DeleteTable(string name)
    {
        return HandleInTransaction(() =>
        {
            var realName = _connection.QueryFirstOrDefault(
                "SELECT `tablerealname` FROM `usertables` WHERE `tabledisplayname` = @name",
                new { name });
            if(realName is null) return false;
            _connection.Execute(
                "DELETE FROM `usertablescolumns` WHERE `tablerealname` = @name",
                new { name = realName.tablerealname });
            int result = _connection.Execute(
                "DELETE FROM `usertables` WHERE `tablerealname` = @name",
                new { name = realName.tablerealname });
            _connection.Execute($"DROP TABLE {realName.tablerealname}");
            return result == 1;
        });
    }
    
    public bool IsTableExists(string name, string? schema = null)
    {
        if(schema is not null)
            return _connection.Database == schema && _isTableExists(name);
        return _isTableExists(name);
    }

    internal bool _isTableExists(string name)
    {
        var realname = _connection.QueryFirstOrDefault(
            "SELECT `tablerealname` FROM `usertables` WHERE `tabledisplayname` = @name",
            new { name });
        if(realname is null) return false;
        return _connection.Query(
                "SELECT * FROM information_schema.TABLES WHERE TABLE_NAME = @name", 
                new { name = realname.tablerealname })
            .FirstOrDefault() is not null;
    }

    public bool CreateColumn(string tablename, string colname, Type type, string color = "#000000")
    {
        if(!_isTableExists(tablename)) throw new ArgumentException("Table does not exist");
        
        string cstype, sqltype;
        if(!_typeMap.ContainsKey(type)) throw new ArgumentException("Unsupported type");
        (cstype, sqltype) = _typeMap[type];
        string realName = _generateRealName(true);

        return HandleInTransaction(() =>
        {
            var tablerealname = _connection.QueryFirstOrDefault(
                "SELECT `tablerealname` FROM `usertables` WHERE `tabledisplayname` = @tablename",
                new { tablename });
            if(tablerealname is null) throw new ArgumentException("Table does not exist");
            int result = _connection.Execute(
                "INSERT INTO `usertablescolumns`" +
                "(`tablerealname`, `realname`, `displayname`, `csharptype`, `color`)" +
                 "VALUES (@tablerealname, @realname, @displayname, @cstype, @color)",
                new { tablerealname.tablerealname,
                            realname = realName,
                            displayname = colname,
                            cstype,
                            color });
            _connection.Execute(
                $"ALTER TABLE `{tablerealname.tablerealname}` ADD COLUMN `{realName}` {sqltype}");
            return result == 1;
        });
    }

    public bool DeleteColumn(string tabledisplayname, string colname)
    {
        if(!_isTableExists(tabledisplayname)) throw new ArgumentException("Table does not exist");

        return HandleInTransaction(() =>
        {
            var realnames = _connection.QueryFirstOrDefault(
                "SELECT ut.tablerealname trn, utc.realname crn" +
                " FROM `usertables` AS `ut` INNER JOIN `usertablescolumns` AS `utc` ON ut.tablerealname = utc.tablerealname" +
                " WHERE `tabledisplayname` = @tabledisplayname AND `displayname` = @colname",
                new { tabledisplayname, colname });
            if(realnames is null) throw new ArgumentException("Column does not exist");
            int result = _connection.Execute(
                "DELETE FROM `usertablescolumns` WHERE tablerealname=@tablename AND realname=@realname",
                new { tablename = realnames.trn, realname = realnames.crn});
            _connection.Execute($"ALTER TABLE `{realnames.trn}` DROP COLUMN `{realnames.crn}`");
            return result == 1;
        });
    }
    
    public bool Insert(InsertQuery insertQuery)
    {
        if(!_isTableExists(insertQuery.TableDisplayName)) throw new ArgumentException("Table does not exist");
        var tablerealname = _connection.QueryFirstOrDefault(
            "SELECT `tablerealname` FROM `usertables` WHERE `tabledisplayname` = @tablename",
            new { tablename = insertQuery.TableDisplayName });
        if(tablerealname is null) throw new ArgumentException("Table does not exist");
        var columns = _connection.Query(
            "SELECT `realname`, `displayname`, `csharptype` FROM `usertablescolumns` WHERE `tablerealname` = @tablename",
            new { tablename = tablerealname.tablerealname });
        if(columns is null) throw new ArgumentException("Table does not exist");
        var values = new Dictionary<string, object>();
        foreach(var column in columns)
        {
            if(!insertQuery.Parameters.ContainsKey(column.displayname))
                throw new ArgumentException("Missing value for column");
            values.Add(column.realname, insertQuery.Parameters[column.displayname]);
        }
        string sql = $"INSERT INTO `{tablerealname.tablerealname}` (" +
            string.Join(", ", values.Keys) +
            ") VALUES (" +
            string.Join(", ", values.Keys.Select(k => "@" + k)) +
            ")";
        return HandleInTransaction(() =>
        {
            int result = _connection.Execute(sql, values);
            return result == 1;
        });
    }

    public ICollection<Table> ShowTables()
    {
        Dictionary<string, Table> tables = new Dictionary<string, Table>();
        var mapping = _connection.Query(
            "SELECT `tabledisplayname` tdn, `displayname` cdn, `color`, `csharptype`" +
            " FROM `usertables` INNER JOIN usertablescolumns u on usertables.tablerealname = u.tablerealname");
        foreach (var record in mapping)
        {
            if(!tables.ContainsKey(record.tdn))
                tables.Add(record.tdn, new Table{DisplayName = record.tdn, Columns = new List<Column>()});
            Table t = tables[record.tdn];
            t.Columns.Add(new Column
            {
                DisplayName = record.cdn,
                Table = t,
                Color = Color.FromArgb(int.Parse(record.color.Substring(0, 2), NumberStyles.HexNumber),
                                      int.Parse(record.color.Substring(2, 2), NumberStyles.HexNumber),
                                      int.Parse(record.color.Substring(4, 2), NumberStyles.HexNumber)),
                Type = _reverseTypeMap[record.csharptype]
            });
        }
        return tables.Values;
    }
    
    public ICollection<Column> ShowColumns(string tablename)
    {
        if(!_isTableExists(tablename)) throw new ArgumentException("Table does not exist");
        var tablerealname = _connection.QueryFirst(
            "SELECT `tablerealname` FROM `usertables` WHERE `tabledisplayname` = @tablename",
            new { tablename });
        var mapping = _connection.Query(
            "SELECT `color`, `displayname`, `csharptype`" +
            " FROM `usertablescolumns` WHERE `tablerealname` = @tablename",
            new { tablename = tablerealname.tablerealname });
        Table t = new Table{DisplayName = tablename, Columns = new List<Column>()};
        foreach (var record in mapping)
        {
            t.Columns.Add(new Column
            {
                DisplayName = record.displayname,
                Table = t,
                Color = Color.FromArgb(int.Parse(record.color.Substring(0, 2), NumberStyles.HexNumber),
                                    int.Parse(record.color.Substring(2, 2), NumberStyles.HexNumber),
                                    int.Parse(record.color.Substring(4, 2), NumberStyles.HexNumber)),
                Type = _reverseTypeMap[record.csharptype]
            });
        }

        return t.Columns;
    }

    [Obsolete("Handle it in your own code")]
    public bool ProcessLogin(string login, string password)
    {
        return HandleInTransaction(() =>
        {
            var user = _connection.QueryAsync($"SELECT login, passwd FROM users where login = '{login}'").Result.FirstOrDefault();
            if(user != null)
            {
                
                if (user.passwd == password) return true;
                else return false;
            }
            else
            {
                return false;
            }
        });
    }

    [Obsolete("Handle it using GetConnection, not implemented - placeholder")]
    public bool Select(SelectQuery query)
    {
        throw new NotImplementedException();
    }
    
    [Obsolete("Handle it using GetConnection, not implemented - placeholder")]
    public bool Update(UpdateQuery query)
    {
        throw new NotImplementedException();
    }
    
    [Obsolete("Handle it using GetConnection, not implemented - placeholder")]
    public bool Delete(DeleteQuery query)
    {
        throw new NotImplementedException();
    }
    
    public MySqlConnection GetConnection()
    {
        return _connection;
    }

}