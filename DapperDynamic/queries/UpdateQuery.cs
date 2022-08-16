namespace DapperDynamic.queries;

public class UpdateQuery : IStatement
{
    IDictionary<string, NameType> IStatement.GetNamesToTranslate()
    {
        throw new NotImplementedException();
    }

    Tuple<string, IDictionary<string, object>> IStatement.GetStatement(IDictionary<string, string> dictionary, bool topLevel)
    {
        throw new NotImplementedException();
    }
}