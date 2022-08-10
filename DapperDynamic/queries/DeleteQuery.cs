namespace DapperDynamic.queries;

public class DeleteQuery : IStatement
{
    IDictionary<string, NameType> IStatement.GetNamesToTranslate()
    {
        throw new NotImplementedException();
    }

    Tuple<string, IDictionary<string, object>> IStatement.GetStatement(IDictionary<string, string> dictionary)
    {
        throw new NotImplementedException();
    }
}