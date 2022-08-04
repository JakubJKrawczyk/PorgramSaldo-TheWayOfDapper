namespace DapperDynamic.queries;

public class WhereChain{}

class WhereChainSingle : WhereChain
{
    internal string? TableName { get; set; }
    internal bool? NeedTranslateTn { get; set; }
    internal string ColumnName { get; set; } = null!;
    internal bool NeedTranslateCn { get; set; }
    internal WhereStatement.Operator Operator { get; set; }
    internal object? Value { get; set; }
    internal WhereStatement.Comparison? BeforeComparison { get; set; } = null;
}