namespace DapperDynamic.queries;

//TODO: Zaimplementować klasę DeleteQuery, tak aby można było usuwać dane z konkretnych tabel, wiersze, kolumny, tabele
public class DeleteQuery : IStatement
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