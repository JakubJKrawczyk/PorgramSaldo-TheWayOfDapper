namespace DapperDynamic.queries;

//Interfejs implementowany przez każde Query.
enum NameType
{
    Column, Table
}

interface IStatement
{
    IDictionary<string, NameType> GetNamesToTranslate();
    Tuple<string, IDictionary<string, object>> GetStatement(IDictionary<string, string> dictionary, bool topLevel=true);
}