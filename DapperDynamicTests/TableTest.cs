using DapperDynamic;
using DapperDynamic.queries;
using DapperDynamic.structures;
using NUnit.Framework;
using System.Drawing;

namespace DapperDynamicTests;

public class TableTests
{
    private DatabaseManager _db;

    [SetUp]
    public void Setup()
    {
        DatabaseManager.Initialize("Saldo", "Kasia!@#", "dapperdynamic");
        _db = DatabaseManager.DBManagerInstance!;
    }

    [Test]
    public void TableCycle()
    {
        bool create = _db.CreateTable("testcycle");
        Assert.IsTrue(create);
        bool drop = _db.DeleteTable("testcycle");
        Assert.IsTrue(drop);
    }

    [Test]
    public void ColumnTest()
    {
        _db.CreateTable("testcolumn", true);
        bool create = _db.CreateColumn("testcolumn", "column1", typeof(double), "00FF00");
        Assert.IsTrue(create);
        bool drop = _db.DeleteColumn("testcolumn", "column1");
        _db.DeleteTable("testcolumn");
        Assert.IsTrue(drop);
    }

    [Test]
    public void InsertTest()
    {
        _db.CreateTable("testinsert", true);
        bool create = _db.CreateColumn("testinsert", "column1", typeof(double), "00FF00");
        Assert.IsTrue(create);
        InsertQuery insert = new InsertQuery("testinsert").Into("column1", 1.0);
        Assert.IsTrue(_db.Insert(insert));
    }

    [Test]
    public void IsTableExists()
    {
        _db.CreateTable("testexists", true);
        bool exists = _db.IsTableExists("testexists");
        _db.DeleteTable("testexists");
        Assert.IsTrue(exists);
        Assert.IsFalse(_db.IsTableExists("testexists"));
    }

    [Test]
    public void ShowTables()
    {
        _db.CreateTable("testshowA", true);
        _db.CreateColumn("testshowA", "column1", typeof(double), "00FF00");
        _db.CreateTable("testshowB", true);
        _db.CreateColumn("testshowB", "column1", typeof(double), "00FF00");
        _db.CreateColumn("testshowB", "column2", typeof(double), "FF0000");
        ICollection<Table> tables = _db.ShowTables();
        _db.DeleteTable("testshowA");
        _db.DeleteTable("testshowB");
        Table a = tables.FirstOrDefault(t => t.DisplayName == "testshowA");
        Assert.IsNotNull(a);
        Assert.That(a.Columns.Count, Is.EqualTo(1));
        Table b = tables.FirstOrDefault(t => t.DisplayName == "testshowB");
        Assert.IsNotNull(b);
        Assert.That(b.Columns.Count, Is.EqualTo(2));
    }

    [Test]
    public void ShowColumns()
    {
        _db.CreateTable("testshowcolumns", true);
        _db.CreateColumn("testshowcolumns", "column1", typeof(double), "00FF00");
        _db.CreateColumn("testshowcolumns", "column2", typeof(double), "FF0000");
        ICollection<Column> columns = _db.ShowColumns("testshowcolumns");
        _db.DeleteTable("testshowcolumns");
        Assert.That(columns.Count, Is.EqualTo(2));
        Column a = columns.FirstOrDefault(c => c.DisplayName == "column1");
        Assert.IsNotNull(a);
        Assert.That(a.Type, Is.EqualTo(typeof(double)));
        Assert.That(a.Color, Is.EqualTo(Color.FromArgb(0, 255, 0)));
        Column b = columns.FirstOrDefault(c => c.DisplayName == "column2");
        Assert.IsNotNull(b);
        Assert.That(b.Type, Is.EqualTo(typeof(double)));
        Assert.That(b.Color, Is.EqualTo(Color.FromArgb(255, 0, 0)));
    }

    [Test]
    public void SelectTest()
    {
        _db.CreateTable("testselect", true);
        _db.CreateColumn("testselect", "name", typeof(string), "00FF00");
        _db.CreateColumn("testselect", "age", typeof(int), "FF0000");
        _db.Insert(new InsertQuery("testselect").Into("name", "John").Into("age", 20));
        _db.Insert(new InsertQuery("testselect").Into("name", "Henry").Into("age", 40));
        var selectQuery = new SelectQuery()
            .Select("name", true, "name")
            .Select("age", true, "age")
            .From(new FromStatement("testselect"));
        var result = _db.Select(selectQuery).ToArray();
        Assert.That(result[0].name, Is.EqualTo("John"));
        Assert.That(result[0].age, Is.EqualTo(20));
        Assert.That(result[1].name, Is.EqualTo("Henry"));
        Assert.That(result[1].age, Is.EqualTo(40));
    }
}