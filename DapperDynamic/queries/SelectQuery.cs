namespace DapperDynamic.queries;

public class SelectQuery : IStatement
{
    IDictionary<string, NameType> IStatement.GetNamesToTranslate()
    {
        throw new NotImplementedException();
    }

    Tuple<string, IDictionary<string, object>> IStatement.GetStatement(IDictionary<string, string> dictionary)
    {
        throw new NotImplementedException();
    }

    public SelectQuery()
    {
        throw new NotImplementedException();
    }

    public SelectQuery(List<Tuple<string, bool>> columns)
    {
        throw new NotImplementedException();
    }
    
    public SelectQuery(List<Tuple<string, bool, string, bool>> columns)
    {
        throw new NotImplementedException();
    }
    
    public SelectQuery From(FromStatement from)
    {
        throw new NotImplementedException();
    }
    
    public SelectQuery Where(WhereStatement where)
    {
        throw new NotImplementedException();
    }
}