using Testcontainers.PostgreSql;

namespace EventHouse.Management.Infrastructure.Tests.Persistence;

public class SharedDatabaseFixture : IAsyncLifetime
{
    private readonly PostgreSqlContainer _dbContainer = new PostgreSqlBuilder("postgres:16-alpine")
        .WithDatabase("eventhouse_shared_tests")
        .Build();

    public string GetConnectionString() => _dbContainer.GetConnectionString();

    public async ValueTask InitializeAsync() => await _dbContainer.StartAsync();

    public async ValueTask DisposeAsync()
    {
        await _dbContainer.StopAsync();

        // Fix for CA1816: Properly handle the garbage collector
        GC.SuppressFinalize(this);
    }
}


[CollectionDefinition("DatabaseCollection")]
public class DatabaseCollection : ICollectionFixture<SharedDatabaseFixture> { }