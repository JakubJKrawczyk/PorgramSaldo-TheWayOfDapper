namespace DapperDynamic.queries;

public class InsertQuery : IStatement
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

    IDictionary<string, NameType> IStatement.GetNamesToTranslate()
    {
        throw new NotImplementedException();
    }

    Tuple<string, IDictionary<string, object>> IStatement.GetStatement(IDictionary<string, string> dictionary, bool topLevel)
    {
        throw new NotImplementedException();
    }
}