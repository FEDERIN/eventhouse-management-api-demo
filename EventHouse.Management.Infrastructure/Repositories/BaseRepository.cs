using EventHouse.Management.Application.Exceptions;
using EventHouse.Management.Infrastructure.Persistence;
using EventHouse.Management.Infrastructure.Persistence.Exceptions;
using Microsoft.EntityFrameworkCore;

namespace EventHouse.Management.Infrastructure.Repositories;

public abstract class BaseRepository(ManagementDbContext context)
{
    protected readonly ManagementDbContext _context = context;

    protected async Task SaveChangesWithUniqueCheckAsync(
        Dictionary<string, (string? Code, string? Detail, bool ShouldIgnore)> indexMappings,
        CancellationToken ct)
    {
        try
        {
            await _context.SaveChangesAsync(ct);
        }
        catch (DbUpdateException ex)
        {
            foreach (var mapping in indexMappings)
            {
                if (ex.IsUniqueViolation(mapping.Key))
                {
                    if (mapping.Value.ShouldIgnore)
                    {
                        _context.ChangeTracker.Clear();
                        return;
                    }

                    throw new ConflictException(
                       code: mapping.Value.Code ?? "UNIQUE_VIOLATION",
                       title: "Unique constraint violated",
                       detail: mapping.Value.Detail ?? "A record already exists."
                   );
                }
            }
            throw;
        }
    }
}