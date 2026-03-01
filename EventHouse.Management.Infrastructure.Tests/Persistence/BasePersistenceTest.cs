using EventHouse.Management.Infrastructure.Persistence;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;

namespace EventHouse.Management.Infrastructure.Tests.Persistence;

public abstract class BasePersistenceTest : IDisposable
{
    protected readonly ManagementDbContext Context;
    private readonly SqliteConnection _connection;

    protected BasePersistenceTest()
    {
        _connection = new SqliteConnection("DataSource=:memory:");
        _connection.Open();

        var options = new DbContextOptionsBuilder<ManagementDbContext>()
            .UseSqlite(_connection)
            .EnableSensitiveDataLogging()
            .Options;

        Context = new ManagementDbContext(options);

        Context.Database.EnsureCreated();
    }

    protected async Task SeedAsync<T>(params T[] entities) where T : class
    {
        await Context.Set<T>().AddRangeAsync(entities);
        await Context.SaveChangesAsync();

        Context.ChangeTracker.Clear();
    }

    public void Dispose()
    {
        Context.Dispose();
        _connection.Dispose();
        GC.SuppressFinalize(this);
    }
}