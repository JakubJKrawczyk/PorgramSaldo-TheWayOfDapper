namespace DapperDynamic.queries;

public class SelectQuery : IStatement
{

    //TODO: Przebudować klasę SelectQuery na prostrzą do zaimplementowania


    private WhereStatement? _where;
    private FromStatement? _from;
    private readonly Dictionary<string, Tuple<bool, string?>> _selects;

    IDictionary<string, NameType> IStatement.GetNamesToTranslate()
    {
        var ret = new Dictionary<string, NameType>();
        if(_from == null) throw new InvalidOperationException("From statement is required");
        foreach(var (k, v) in ((IStatement)_from).GetNamesToTranslate())
        {
            ret.Add(k, v);
        }
        
        foreach (var (column, (needTranslate, _)) in _selects)
        {
            if(ret.ContainsKey(column) && ret[column] != NameType.Column)
                throw new InvalidOperationException($"Conflicting name {column}");
            if(ret.ContainsKey(column)) continue;
            if(!needTranslate) continue;
            ret.Add(column, NameType.Column);
        }

        if (_where == null) return ret;
        foreach (var (k, v) in ((IStatement)_where).GetNamesToTranslate())
        {
            if(ret.ContainsKey(k) && ret[k] != v) throw new InvalidOperationException($"Conflicting name {k}");
            if(ret.ContainsKey(k)) continue;
            ret.Add(k, v);
        }
        return ret;
    }

    Tuple<string, IDictionary<string, object>> IStatement.GetStatement(IDictionary<string, string> dictionary, bool topLevel)
    {
        if(_from is null) throw new InvalidOperationException("From statement is required");
        string sql = "";
        var parameters = new Dictionary<string, object>();
        if (_selects.Count == 0) sql += "*";
        else
        {
            List<string> columns = new List<string>();
            foreach (var (column, (needTranslate, alias)) in _selects)
            {
                columns.Add($"{(needTranslate ? dictionary[column] : column)}{(alias is null ? "" : $" AS {alias}")}");
            }
            sql += string.Join(", ", columns);
        }
        sql += $" {((IStatement)_from).GetStatement(dictionary).Item1}";
        
        if(_where != null)
        {
            var where = ((IStatement)_where).GetStatement(dictionary, false);
            if(where.Item2.Count > 0)
            {
                sql += $" WHERE {where.Item1}";
            }
            foreach(var (k, v) in where.Item2)
            {
                parameters.Add(k, v);
            }
        }
        return new Tuple<string, IDictionary<string, object>>("SELECT " + sql, parameters);
    }
    
    internal IList<string> GetInvolvedTables()
    {
        if(_from is null) throw new InvalidOperationException("From statement is required");
        var fromNames = ((IStatement)_from).GetNamesToTranslate();
        var ret = new List<string>();
        foreach(var (k, type) in fromNames)
        {
            if(type == NameType.Table)
                ret.Add(k);
        }
        return ret;
    }

    public SelectQuery()
    {
        _from = null;
        _where = null;
        _selects = new Dictionary<string, Tuple<bool, string?>>();
    }
    
    public SelectQuery From(FromStatement from)
    {
        _from = from;
        return this;
    }
    
    public SelectQuery Where(WhereStatement where)
    {
        _where = where;
        return this;
    }

    public SelectQuery Select(string columnOrAlias, bool toTranslate = true, string? alias = null)
    {
        _selects.Add(columnOrAlias, new Tuple<bool, string?>(toTranslate, alias));
        return this;
    }
}