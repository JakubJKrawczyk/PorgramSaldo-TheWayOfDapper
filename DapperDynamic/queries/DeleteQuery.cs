namespace DapperDynamic.queries;

public class DeleteQuery : IStatement
{
    public DeleteQuery(string table)
    {
        throw new NotImplementedException();
    }

    IDictionary<string, NameType> IStatement.GetNamesToTranslate()
    {
        throw new NotImplementedException();
    }

    Tuple<string, IDictionary<string, object>> IStatement.GetStatement(IDictionary<string, string> dictionary, bool topLevel)
    {
        throw new NotImplementedException();
    }

    public DeleteQuery Where(WhereStatement where)
    {
        throw new NotImplementedException();
    }
}