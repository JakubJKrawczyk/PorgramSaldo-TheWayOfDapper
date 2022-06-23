using DapperDynamic;
using NUnit.Framework;

namespace DapperDynamicTests;

public class TableTests
{
    private DatabaseManager _db;
    
    [SetUp]
    public void Setup()
    {
        DatabaseManager.Initialize("root", "", "dapperdynami");
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
        _db.CreateTable("testcolumn");
        bool create = _db.CreateColumn("testcolumn", "column1", typeof(double), "#00FF00");
        Assert.IsTrue(create);
        bool drop = _db.DeleteColumn("testcolumn", "column1");
        Assert.IsTrue(drop);
    }
}