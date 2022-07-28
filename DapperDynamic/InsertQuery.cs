namespace DapperDynamic;

public class InsertQuery
{
    internal string TableDisplayName;
    
    internal Dictionary<string, Object> Parameters = new();

    public InsertQuery(string tableName)
    {
        TableDisplayName = tableName;
    }

    public InsertQuery Into(string columnName, object value)
    {
        Parameters.Add(columnName, value);
        return this;
    }
}