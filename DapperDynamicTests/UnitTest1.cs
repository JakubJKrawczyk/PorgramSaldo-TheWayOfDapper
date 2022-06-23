using DapperDynamic;
using NUnit.Framework;

namespace DapperDynamicTests;

public class Tests
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
}