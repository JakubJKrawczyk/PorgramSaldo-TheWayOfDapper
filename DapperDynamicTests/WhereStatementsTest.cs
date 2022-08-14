using DapperDynamic.queries;
using NUnit.Framework;

namespace DapperDynamicTests;

public class WhereStatementsTest
{
    [Test]
    public void WhereStatementSimple()
    {
        WhereStatement where = new WhereStatement();
        where.Where("id", WhereStatement.Operator.Equals, 1);
        var ttr = ((IStatement)where).GetNamesToTranslate();
        Assert.IsTrue(ttr.ContainsKey("id"));
        Tuple<string, IDictionary<string, object>> sql = ((IStatement)where).GetStatement(new Dictionary<string, string>()
        {
            {"id", "col1"}
        });
        Assert.That(sql.Item1, Is.EqualTo("WHERE col1 = @id_0"));
        Assert.True(sql.Item2.ContainsKey("id_0"));
        Assert.That(sql.Item2["id_0"], Is.EqualTo(1));
    }

    [Test]
    public void WhereStatementSimpleMultiple()
    {
        WhereStatement where = new WhereStatement();
        where.Where("id", WhereStatement.Operator.Equals, 1);
        where.AndWhere("name", WhereStatement.Operator.Equals, "test");
        where.OrWhere("count", WhereStatement.Operator.GreaterThan, 100);
        var ttr = ((IStatement)where).GetNamesToTranslate();
        Assert.IsTrue(ttr.ContainsKey("id"));
        Assert.IsTrue(ttr.ContainsKey("name"));
        Assert.IsTrue(ttr.ContainsKey("count"));
        Tuple<string, IDictionary<string, object>> sql = ((IStatement)where).GetStatement(new Dictionary<string, string>()
        {
            {"id", "col1"},
            {"name", "col2"},
            {"count", "col3"}
        });
        Assert.That(sql.Item1, Is.EqualTo("WHERE col1 = @id_0 and col2 = @name_0 or col3 > @count_0"));
        Assert.True(sql.Item2.ContainsKey("id_0"));
        Assert.True(sql.Item2.ContainsKey("name_0"));
        Assert.True(sql.Item2.ContainsKey("count_0"));
        Assert.That(sql.Item2["id_0"], Is.EqualTo(1));
        Assert.That(sql.Item2["name_0"], Is.EqualTo("test"));
        Assert.That(sql.Item2["count_0"], Is.EqualTo(100));
    }

    [Test]
    public void WhereStatementComplex()
    {
        WhereStatement whereOuter = new WhereStatement();
        whereOuter.Where("id", WhereStatement.Operator.Equals, 1);
        WhereStatement whereInner = new WhereStatement();
        whereInner.Where("name", WhereStatement.Operator.Equals, (object)"test");
        whereInner.AndWhere("count", WhereStatement.Operator.GreaterThan, 100);
        whereOuter.OrWhere(whereInner);
        var tts = ((IStatement)whereOuter).GetNamesToTranslate();
        Assert.IsTrue(tts.ContainsKey("id"));
        Assert.IsTrue(tts.ContainsKey("name"));
        Assert.IsTrue(tts.ContainsKey("count"));
        Tuple<string, IDictionary<string, object>> sql = ((IStatement)whereOuter).GetStatement(new Dictionary<string, string>()
        {
            {"id", "col1"},
            {"name", "col2"},
            {"count", "col3"}
        });
        Assert.That(sql.Item1, Is.EqualTo("WHERE col1 = @id_0 or (col2 = @name_0_0 and col3 > @count_0_0)"));
        Assert.True(sql.Item2.ContainsKey("id_0"));
        Assert.True(sql.Item2.ContainsKey("name_0_0"));
        Assert.True(sql.Item2.ContainsKey("count_0_0"));
        Assert.That(sql.Item2["id_0"], Is.EqualTo(1));
        Assert.That(sql.Item2["name_0_0"], Is.EqualTo("test"));
        Assert.That(sql.Item2["count_0_0"], Is.EqualTo(100));
    }

    [Test]
    public void WhereStatementWithNull()
    {
        WhereStatement where = new WhereStatement();
        where.Where("id", WhereStatement.Operator.Equals, 1);
        where.AndWhere("name", WhereStatement.Operator.Equals, null);
        var statement = ((IStatement)where).GetStatement(new Dictionary<string, string>()
        {
            {"id", "col1"},
            {"name", "col2"}
        });
        Assert.That(statement.Item1, Is.EqualTo("WHERE col1 = @id_0 and col2 is null"));
        Assert.True(statement.Item2.ContainsKey("id_0"));
        Assert.That(statement.Item2["id_0"], Is.EqualTo(1));
    }

    [Test]
    public void WhereStatementWithAliases()
    {
        WhereStatement where = new WhereStatement();
        where.Where("id", WhereStatement.Operator.Equals, 1);
        where.AndWhere("name", WhereStatement.Operator.NotEquals, "test", 
            "q1", true, true);
        where.OrWhere("count", WhereStatement.Operator.GreaterThan, 100,
                    "q2", true);
        var tts = ((IStatement)where).GetNamesToTranslate();
        Assert.IsTrue(tts.ContainsKey("id"));
        Assert.IsFalse(tts.ContainsKey("name"));
        Assert.IsFalse(tts.ContainsKey("count"));
        Assert.IsFalse(tts.ContainsKey("q1"));
        Assert.IsTrue(tts.ContainsKey("q2"));
        Tuple<string, IDictionary<string, object>> sql = ((IStatement)where).GetStatement(new Dictionary<string, string>()
        {
            {"id", "col1"},
            {"q2", "tab1"}
        });
        Assert.That(sql.Item1, Is.EqualTo("WHERE col1 = @id_0 and q1.name != @name_0 or tab1.count > @count_0"));
    }

    [Test]
    public void WhereStatementWithNotNull()
    {
        WhereStatement where = new WhereStatement();
        where.Where("id", WhereStatement.Operator.Equals, 1);
        where.AndWhere("name", WhereStatement.Operator.NotEquals, null);
        var statement = ((IStatement)where).GetStatement(new Dictionary<string, string>()
        {
            {"id", "col1"},
            {"name", "col2"}
        });
        Assert.That(statement.Item1, Is.EqualTo("WHERE col1 = @id_0 and col2 is not null"));
        Assert.True(statement.Item2.ContainsKey("id_0"));
        Assert.That(statement.Item2["id_0"], Is.EqualTo(1));
    }

    [Test]
    public void WhereStatementCompareColumns()
    {
        WhereStatement where = new WhereStatement();
        where.Where("ready_date", WhereStatement.Operator.GreaterThan, "start_date");
        var statement = ((IStatement)where).GetStatement(new Dictionary<string, string>()
        {
            {"ready_date", "col1"},
            {"start_date", "col2"}
        });
        Assert.That(statement.Item1, Is.EqualTo("WHERE col1 > col2"));
    }

    [Test]
    public void WhereStatementColumnsFormDifferentTables()
    {
        WhereStatement where = new WhereStatement();
        where.Where("id", "order", WhereStatement.Operator.Equals, "id", "order_item");
        var names = ((IStatement)where).GetNamesToTranslate();
        Assert.IsTrue(names.ContainsKey("order.id"));
        Assert.IsTrue(names.ContainsKey("order_item.id"));
        Assert.IsTrue(names.ContainsKey("order"));
        Assert.IsTrue(names.ContainsKey("order_item"));
        Tuple<string, IDictionary<string, object>> sql = ((IStatement)where).GetStatement(new Dictionary<string, string>()
        {
            {"order.id", "col1"},
            {"order_item.id", "col2"},
            {"order", "tab1"},
            {"order_item", "tab2"}
        });
        Assert.That(sql.Item1, Is.EqualTo("WHERE tab1.col1 = tab2.col2"));
    }
}