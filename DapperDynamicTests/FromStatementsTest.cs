using DapperDynamic.queries;
using NUnit.Framework;

namespace DapperDynamicTests;

public class FromStatementsTest
{
    [Test]
    public void FromStatementSimple()
    {
        FromStatement from = new FromStatement("tabela");
        var ttr = ((IStatement)from).GetNamesToTranslate();
        Assert.IsTrue(ttr.ContainsKey("tabela"));
        Tuple<string, IDictionary<string, object>> sql = ((IStatement)from).GetStatement(new Dictionary<string, string>()
        {
            {"tabela", "tb1"}
        });
        Assert.That(sql.Item1, Is.EqualTo("FROM tb1"));
        Assert.True(sql.Item2.Count == 0);
    }

    [Test]
    public void FromStatementSimpleMultiple()
    {
        FromStatement from = new FromStatement("products");
        from.InnerJoin("categories", new WhereStatement().Where(
            "category_id", "products", WhereStatement.Operator.Equals,
            "category_id", "categories"));
        var ttr = ((IStatement)from).GetNamesToTranslate();
        Assert.IsTrue(ttr.ContainsKey("categories"));
        Assert.IsTrue(ttr.ContainsKey("products"));
        Assert.IsTrue(ttr.ContainsKey("categories.category_id"));
        Assert.IsTrue(ttr.ContainsKey("products.category_id"));
        Tuple<string, IDictionary<string, object>> sql = ((IStatement)from).GetStatement(new Dictionary<string, string>()
        {
            {"categories", "tab1"},
            {"products", "tab2"},
            {"categories.category_id", "col1"},
            {"products.category_id", "col2"}
        });
        Assert.That(sql.Item1, Is.EqualTo("FROM tab1 INNER JOIN tab2 ON tab1.col1 = tab2.col2"));
    }

    [Test]
    public void FromStatementComplex()
    {
        SelectQuery select = new SelectQuery()
            .From(new FromStatement("products"))
            .Select("product_name").Select("product_id");
        FromStatement fromOuter = new FromStatement(select, "q1");
        var tts = ((IStatement)fromOuter).GetNamesToTranslate();
        Assert.IsTrue(tts.ContainsKey("products"));
        Assert.IsTrue(tts.ContainsKey("products.product_id"));
        Assert.IsTrue(tts.ContainsKey("products.product_name"));
        Tuple<string, IDictionary<string, object>> sql = ((IStatement)fromOuter).GetStatement(new Dictionary<string, string>()
        {
            {"products", "tab1"},
            {"products.product_id", "col2"},
            {"products.product_name", "col3"}
        });
        Assert.That(sql.Item1, Is.EqualTo("FROM (SELECT col3, col2 FROM tab1) AS q1"));
    }

    [Test]
    public void WhereStatementWithAliases()
    {
        FromStatement where = new FromStatement("products");
        where.RightJoin("categories", new WhereStatement().Where(
            "category_id", "products", WhereStatement.Operator.Equals,
            "category_id", "q1",
            false, false,
            false, true), "q1");
        var tts = ((IStatement)where).GetNamesToTranslate();
        Assert.IsTrue(tts.ContainsKey("products"));
        Assert.IsFalse(tts.ContainsKey("products.category_id"));
        Assert.IsFalse(tts.ContainsKey("q1.category_id"));
        Tuple<string, IDictionary<string, object>> sql = ((IStatement)where).GetStatement(new Dictionary<string, string>()
        {
            {"products", "tab1"},
            {"products.category_id", "col1"},
            {"q1.category_id", "col2"}
        });
        Assert.That(sql.Item1, Is.EqualTo("FROM tab1 RIGHT JOIN categories AS q1 ON tab1.col1 = q1.col2"));
    }
}