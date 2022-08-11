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
        Assert.That(statement.Item1, Is.EqualTo("SELECT * FROM tab1 WHERE col1 = @Id_0"));
        Assert.IsTrue(statement.Item2.ContainsKey("Id_0"));
        Assert.That(statement.Item2["Id_0"], Is.EqualTo(1));
    }
    
    [Test]
    public void SelectWithColumnAliases()
    {
        SelectQuery select = new SelectQuery();
        select.From(new FromStatement("Users"));
        select.Select("Id", true, "UserId");
        select.Select("Name", true, "UserName");
        var names = ((IStatement)select).GetNamesToTranslate();
        Assert.IsTrue(names.ContainsKey("Users"));
        Assert.IsTrue(names.ContainsKey("Id"));
        Assert.IsTrue(names.ContainsKey("Name"));
        var statement = ((IStatement)select).GetStatement(new Dictionary<string, string>()
        {
            { "Users", "tab1" },
            { "Id", "col1" },
            { "Name", "col2" }
        });
        Assert.That(statement.Item1, Is.EqualTo("SELECT col1 AS UserId, col2 AS UserName FROM tab1"));
    }
}