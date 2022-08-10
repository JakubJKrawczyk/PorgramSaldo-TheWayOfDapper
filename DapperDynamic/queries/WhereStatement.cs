namespace DapperDynamic.queries;

public class WhereStatement : WhereChain, IStatement
{
    internal List<WhereChain> WhereChain = new();
    internal Comparison? BeforeComparasion { get; set; }
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
        if(WhereChain.Count != 0) throw new InvalidOperationException("Cannot set first where statement after an other statement");
        WhereChain.Add(nested);
        return this;
    }
    
    public WhereStatement Where(
        string columnOrAlias, Operator op, object value,
        string? tableOrAlias = null, bool isColumnAlias = false, bool isTableAlias = false)
    {
        if(WhereChain.Count != 0) throw new InvalidOperationException("Cannot set first where statement after an other statement");
        WhereChain.Add(new WhereChainSingle
        {
            ColumnName = columnOrAlias, Operator = op, Value = value,
            TableName = tableOrAlias, NeedTranslateCn = !isColumnAlias, NeedTranslateTn = !isTableAlias
        });
        return this;
    }
    
    public WhereStatement OrWhere(WhereStatement nested)
    {
        if(WhereChain.Count == 0) throw new InvalidOperationException("Cannot set next where statement as first");
        nested.BeforeComparasion = Comparison.Or;
        WhereChain.Add(nested);
        return this;
    }

    public WhereStatement OrWhere(
        string columnOrAlias, Operator op, object value,
        string? tableOrAlias = null, bool isColumnAlias = false, bool isTableAlias = false)
    {
        if(WhereChain.Count == 0) throw new InvalidOperationException("Cannot set next where statement as first");
        WhereChainSingle wcs = new WhereChainSingle
        {
            BeforeComparison = Comparison.Or, ColumnName = columnOrAlias, Operator = op, Value = value,
            TableName = tableOrAlias, NeedTranslateCn = !isColumnAlias, NeedTranslateTn = !isTableAlias
        };
        WhereChain.Add(wcs);
        return this;
    }
    
    public WhereStatement AndWhere(WhereStatement nested)
    {
        if(WhereChain.Count == 0) throw new InvalidOperationException("Cannot set next where statement as first");
        nested.BeforeComparasion = Comparison.And;
        WhereChain.Add(nested);
        return this;
    }
    
    public WhereStatement AndWhere(
        string columnOrAlias, Operator op, object value,
        string? tableOrAlias = null, bool isColumnAlias = false, bool isTableAlias = false)
    {
        if(WhereChain.Count == 0) throw new InvalidOperationException("Cannot set next where statement as first");
        WhereChainSingle wcs = new WhereChainSingle
        {
            BeforeComparison = Comparison.And, ColumnName = columnOrAlias, Operator = op, Value = value,
            TableName = tableOrAlias, NeedTranslateCn = !isColumnAlias, NeedTranslateTn = !isTableAlias
        };
        WhereChain.Add(wcs);
        return this;
    }

    IDictionary<string, NameType> IStatement.GetNamesToTranslate()
    {
        throw new NotImplementedException();
    }

    Tuple<string, IDictionary<string, object>> IStatement.GetStatement(IDictionary<string, string> dictionary)
    {
        throw new NotImplementedException();
    }
}