using DapperDynamic.queries;
using NUnit.Framework;

namespace DapperDynamicTests;

public class SelectQueryTest
{
    [Test]
    public void MinimalWorkingTranslation()
    {
        SelectQuery select = new SelectQuery();
        select.From(new FromStatement("Users"));
        select.Where(new WhereStatement().Where("Id", WhereStatement.Operator.Equals, 1));
        var names = ((IStatement)select).GetNamesToTranslate();
        Assert.IsTrue(names.ContainsKey("Users"));
        Assert.IsTrue(names.ContainsKey("Id"));
        var statement = ((IStatement)select).GetStatement(new Dictionary<string, string>()
        {
            { "Users", "tab1" },
            { "Id", "col1" }
        });
        Assert.That(statement.Item1, Is.EqualTo("SELECT * FROM tab1 WHERE col1 = @col1"));
        Assert.IsTrue(statement.Item2.ContainsKey("col1"));
        Assert.That(statement.Item2["col1"], Is.EqualTo(1));
    }
}