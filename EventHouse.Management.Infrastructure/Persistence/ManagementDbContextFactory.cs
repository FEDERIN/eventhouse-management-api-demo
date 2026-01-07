using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace EventHouse.Management.Infrastructure.Persistence;

public sealed class ManagementDbContextFactory
    : IDesignTimeDbContextFactory<ManagementDbContext>
{
    public ManagementDbContext CreateDbContext(string[] args)
    {
        var options = new DbContextOptionsBuilder<ManagementDbContext>()
            .UseSqlite("Data Source=management.db")
            .Options;

        return new ManagementDbContext(options);
    }
}
