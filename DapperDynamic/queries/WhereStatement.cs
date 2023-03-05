namespace DapperDynamic.queries;

public class WhereStatement : WhereChain, IStatement
{

#warning Potrzebujemy tej klasy?
    //TODO: Przystosować klasę WhereStatement do nowo stworzonej klasy SelectQuery, dodać możliwość użycia wielu warunków w zapytaniu.

    private List<WhereChain> WhereChain = new();
    private Comparison? BeforeComparasion { get; set; }
    public enum Operator
    {
        Equals,
        NotEquals,
        GreaterThan,
        GreaterOrEqual,
        LessThan,
        LessOrEqual,
    }

    public enum Comparison
    {
        And,
        Or
    }

    internal static Dictionary<Operator, string> OperatorMap = new()
    {
        {Operator.Equals, "="},
        {Operator.NotEquals, "!="},
        {Operator.GreaterThan, ">"},
        {Operator.GreaterOrEqual, ">="},
        {Operator.LessThan, "<"},
        {Operator.LessOrEqual, "<="},
    };

    public WhereStatement Where(WhereStatement nested)
    {
        if (WhereChain.Count != 0) throw new InvalidOperationException("Cannot set first where statement after an other statement");
        WhereChain.Add(nested);
        return this;
    }

    public WhereStatement Where(
        string columnOrAlias, Operator op, object value,
        string? tableOrAlias = null, bool isColumnAlias = false, bool isTableAlias = false)
    {
        if (WhereChain.Count != 0) throw new InvalidOperationException("Cannot set first where statement after an other statement");
        WhereChain.Add(new WhereChainSingle
        {
            ColumnName = columnOrAlias,
            Operator = op,
            Value = value,
            TableName = tableOrAlias,
            NeedTranslateCn = !isColumnAlias,
            NeedTranslateTn = !isTableAlias
        });
        return this;
    }

    public WhereStatement Where(
        string columnOrAlias1, string tableOrAlias1, Operator op,
        string columnOrAlias2, string tableOrAlias2,
        bool isColumnAlias1 = false, bool isTableAlias1 = false,
        bool isColumnAlias2 = false, bool isTableAlias2 = false)
    {
        throw new NotImplementedException();
    }

    public WhereStatement Where(
        string columnOrAlias1, Operator op, string columnOrAlias2,
        bool isColumnAlias1 = false, bool isColumnAlias2 = false)
    {
        throw new NotImplementedException();
    }

    public WhereStatement OrWhere(WhereStatement nested)
    {
        if (WhereChain.Count == 0) throw new InvalidOperationException("Cannot set next where statement as first");
        nested.BeforeComparasion = Comparison.Or;
        WhereChain.Add(nested);
        return this;
    }

    public WhereStatement OrWhere(
        string columnOrAlias, Operator op, object value,
        string? tableOrAlias = null, bool isColumnAlias = false, bool isTableAlias = false)
    {
        if (WhereChain.Count == 0) throw new InvalidOperationException("Cannot set next where statement as first");
        WhereChainSingle wcs = new WhereChainSingle
        {
            BeforeComparison = Comparison.Or,
            ColumnName = columnOrAlias,
            Operator = op,
            Value = value,
            TableName = tableOrAlias,
            NeedTranslateCn = !isColumnAlias,
            NeedTranslateTn = !isTableAlias
        };
        WhereChain.Add(wcs);
        return this;
    }

    public WhereStatement AndWhere(WhereStatement nested)
    {
        if (WhereChain.Count == 0) throw new InvalidOperationException("Cannot set next where statement as first");
        nested.BeforeComparasion = Comparison.And;
        WhereChain.Add(nested);
        return this;
    }

    public WhereStatement AndWhere(
        string columnOrAlias, Operator op, object value,
        string? tableOrAlias = null, bool isColumnAlias = false, bool isTableAlias = false)
    {
        if (WhereChain.Count == 0) throw new InvalidOperationException("Cannot set next where statement as first");
        WhereChainSingle wcs = new WhereChainSingle
        {
            BeforeComparison = Comparison.And,
            ColumnName = columnOrAlias,
            Operator = op,
            Value = value,
            TableName = tableOrAlias,
            NeedTranslateCn = !isColumnAlias,
            NeedTranslateTn = !isTableAlias
        };
        WhereChain.Add(wcs);
        return this;
    }

    IDictionary<string, NameType> IStatement.GetNamesToTranslate()
    {
        Dictionary<string, NameType> names = new();
        foreach (WhereChain wc in WhereChain)
        {
            if (wc is WhereChainSingle wcs)
            {
                if (wcs.NeedTranslateCn) names.Add(wcs.ColumnName, NameType.Column);
                if (wcs.TableName != null
                   && wcs.NeedTranslateTn.HasValue
                   && wcs.NeedTranslateTn.Value) names.Add(wcs.TableName, NameType.Table);
            }
            else
            {
                IStatement ws = (WhereStatement)wc;
                foreach (var kvp in ws.GetNamesToTranslate())
                {
                    names.Add(kvp.Key, kvp.Value);
                }
            }
        }
        return names;
    }

    Tuple<string, IDictionary<string, object>> IStatement.GetStatement(IDictionary<string, string> dictionary, bool topLevel)
    {
        string statement = "";
        IDictionary<string, object> parameters = new Dictionary<string, object>();
        IDictionary<string, int> parameterIndex = new Dictionary<string, int>();
        foreach (WhereChain wc in WhereChain)
        {
            if (wc is WhereChainSingle wcs)
            {
                if (!parameterIndex.ContainsKey(wcs.ColumnName)) parameterIndex.Add(wcs.ColumnName, 0);
                if (wcs.BeforeComparison is not null)
                    statement += $" {wcs.BeforeComparison.Value.ToString().ToLower()} ";

                string operatorString = $"{OperatorMap[wcs.Operator]} @{wcs.ColumnName}_{parameterIndex[wcs.ColumnName]}";
                if (wcs.Value is null && wcs.Operator == Operator.Equals) operatorString = "is null";
                else if (wcs.Value is null && wcs.Operator == Operator.NotEquals) operatorString = "is not null";

                string cn = wcs.ColumnName;
                string? tn = wcs.TableName;
                if (wcs.NeedTranslateCn) cn = dictionary[wcs.ColumnName];
                if (wcs.NeedTranslateTn.HasValue && wcs.NeedTranslateTn.Value && wcs.TableName is not null)
                    tn = dictionary[wcs.TableName];
                if (tn is not null) statement += $"{tn}.{cn} {operatorString}";
                else statement += $"{cn} {operatorString}";
                if (wcs.Value is null) continue;
                parameters.Add($"{wcs.ColumnName}_{parameterIndex[wcs.ColumnName]}", wcs.Value);
                parameterIndex[wcs.ColumnName]++;
            }
            else
            {
                var ws = (WhereStatement)wc;
                var (subStatement, subParams) = ((IStatement)ws).GetStatement(dictionary, false);

                foreach (var (paramName, paramValue) in subParams)
                {
                    if (!parameterIndex.ContainsKey(paramName)) parameterIndex.Add(paramName, 0);
                    parameters.Add($"{paramName}_{parameterIndex[paramName]}", paramValue);
                    subStatement = subStatement.Replace(paramName, $"{paramName}_{parameterIndex[paramName]}");
                    parameterIndex[paramName]++;


                    if (ws.BeforeComparasion is null) statement += $"({subStatement})";
                    else statement += $" {ws.BeforeComparasion.Value.ToString().ToLower()} ({subStatement})";
                }
            }
            if (topLevel) statement = $"WHERE {statement}";
            
        }
        return new Tuple<string, IDictionary<string, object>>(statement, parameters); ;
    }
}