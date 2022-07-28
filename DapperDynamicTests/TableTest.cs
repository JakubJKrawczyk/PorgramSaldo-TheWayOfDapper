using DapperDynamic;
using NUnit.Framework;

namespace DapperDynamicTests;

public class TableTests
{
    private DatabaseManager _db;
    
    [SetUp]
    public void Setup()
    {
        DatabaseManager.Initialize("root", "", "dapperdynamic");
        _db = DatabaseManager.Instance!;
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
}