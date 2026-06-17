using EventHouse.Management.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using Respawn;

namespace EventHouse.Management.Infrastructure.Tests.Persistence;

[Collection("DatabaseCollection")]
public abstract class BasePersistenceTest : IAsyncLifetime
{
    protected readonly ManagementDbContext Context;
    private readonly string _connectionString;
    private Respawner? _respawner;

    protected BasePersistenceTest(SharedDatabaseFixture fixture)
    {
        _connectionString = fixture.GetConnectionString();

        var options = new DbContextOptionsBuilder<ManagementDbContext>()
            .UseNpgsql(_connectionString)
            .UseSnakeCaseNamingConvention()
            .Options;

        Context = new ManagementDbContext(options);
    }

    public virtual async ValueTask InitializeAsync()
    {
        // Asegura que el esquema existe (solo la primera vez hace algo real)
        await Context.Database.EnsureCreatedAsync();

        // Inicializa Respawn para limpiar las tablas en milisegundos
        var connection = Context.Database.GetDbConnection();
        await connection.OpenAsync();

        _respawner ??= await Respawner.CreateAsync(connection, new RespawnerOptions
        {
            DbAdapter = DbAdapter.Postgres,
            SchemasToInclude = new[] { "public" }
        });

        await _respawner.ResetAsync(connection);
    }

    public virtual async ValueTask DisposeAsync() => await Context.DisposeAsync();

    protected async Task SeedAsync<T>(params T[] entities) where T : class
    {
        await Context.Set<T>().AddRangeAsync(entities);
        await Context.SaveChangesAsync();
        Context.ChangeTracker.Clear();
    }
}