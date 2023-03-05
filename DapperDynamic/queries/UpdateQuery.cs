namespace DapperDynamic.queries;

public class UpdateQuery : IStatement
{

    //TODO: Zaimplementować klasę UpdateQuery
    IDictionary<string, NameType> IStatement.GetNamesToTranslate()
    {
        throw new NotImplementedException();
    }

    Tuple<string, IDictionary<string, object>> IStatement.GetStatement(IDictionary<string, string> dictionary, bool topLevel)
    {
        throw new NotImplementedException();
    }
}