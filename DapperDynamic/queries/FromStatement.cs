using System.Globalization;

namespace DapperDynamic.queries;

//TODO: Zaimplementować klasę FromStatement i dodać możliwość używania wielu źródeł w jednym zapytaniu.

public class FromStatement : IStatement
{
    private string _tableName;
    private bool _isAlias;
    
    public FromStatement (string tableDisplayName, bool isAlias = false)
    {
        _tableName = tableDisplayName;
        _isAlias = isAlias;
    }

    public FromStatement(SelectQuery selectQuery, string alias)
    {
        
    }
    
    public FromStatement InnerJoin(string tableDisplayName, WhereStatement on, string? alias = null)
    {
        return this;
    }
    
    public FromStatement InnerJoin(SelectQuery selectQuery, WhereStatement on, string alias)
    {
        return this;
    }
    
    public FromStatement LeftJoin(string tableDisplayName, WhereStatement on, string? alias = null)
    {
        return this;
    }
    
    public FromStatement LeftJoin(SelectQuery selectQuery, WhereStatement on, string alias)
    {
        return this;
    }
    
    public FromStatement RightJoin(string tableDisplayName, WhereStatement on, string? alias = null)
    {
        return this;
    }
    
    public FromStatement RightJoin(SelectQuery selectQuery, WhereStatement on, string alias)
    {
        return this;
    }
    
    public FromStatement FullJoin(string tableDisplayName, WhereStatement on, string? alias = null)
    {
        return this;
    }
    
    public FromStatement FullJoin(SelectQuery selectQuery, WhereStatement on, string alias)
    {
        return this;
    }

    IDictionary<string, NameType> IStatement.GetNamesToTranslate()
    {
        var names = new Dictionary<string, NameType>();
        if(!_isAlias) names.Add(_tableName, NameType.Table);
        return names;
    }

    Tuple<string, IDictionary<string, object>> IStatement.GetStatement(IDictionary<string, string> dictionary, bool topLevel)
    {
        string statement = "";
        if(_isAlias) statement += _tableName;
        else statement += dictionary[_tableName];
        if(topLevel) statement = "FROM " + statement;
        return new Tuple<string, IDictionary<string, object>>(statement, new Dictionary<string, object>());
    }
}