using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Test.Utils.Data;

namespace Test.Utils.Integration;

public sealed class MemoryDatabase : IDisposable
{
    private readonly SqliteConnection _connection;

    public MemoryDatabase()
    {
        _connection = new SqliteConnection("Filename=:memory:");
        var contextOptions = new DbContextOptionsBuilder<TestContext>().UseSqlite(_connection).Options;
        _connection.Open();

        Context = new TestContext(contextOptions);
        Context.Database.EnsureCreated();
    }

    public TestContext Context { get; }

    public void Dispose() => _connection.Close();
}