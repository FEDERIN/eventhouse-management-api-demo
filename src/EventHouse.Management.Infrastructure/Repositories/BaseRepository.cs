using EventHouse.Management.Application.Exceptions;
using EventHouse.Management.Domain.Entities;
using EventHouse.Management.Infrastructure.Persistence;
using EventHouse.Management.Infrastructure.Persistence.Exceptions;
using EventHouse.ShareKernel.Entities;
using Microsoft.EntityFrameworkCore;

namespace EventHouse.Management.Infrastructure.Repositories;

internal abstract class BaseRepository(ManagementDbContext context)
{
    protected readonly ManagementDbContext _context = context;

    protected abstract IReadOnlyDictionary<string, UniqueConstraintMapping> IndexMappings { get; }

    protected async Task AddAsync<TEntity>(
    TEntity entity,
    CancellationToken ct = default)
    where TEntity : Entity
    {
        await _context.Set<TEntity>().AddAsync(entity, ct);
        await SaveChangesAsync(ct);
    }

    protected async Task UpdateAsync<TEntity>(
    TEntity entity,
    CancellationToken ct = default)
    where TEntity : Entity
    {
        if (_context.Entry(entity).State == EntityState.Detached)
            throw new InvalidOperationException(
                "UpdateAsync requires a tracked entity. Use GetTrackedByIdAsync.");

        await SaveChangesAsync(ct);
    }

    protected async Task<bool> DeleteAsync<TEntity>(
        Guid id,
        CancellationToken ct = default)
        where TEntity : Entity
    {
        var set = _context.Set<TEntity>();

        var entity = await set.FirstOrDefaultAsync(x => x.Id == id, ct);

        if (entity is null)
            return false;

        set.Remove(entity);

        await SaveChangesAsync(ct);

        return true;
    }

    protected Task<TEntity?> GetByIdAsync<TEntity>(
        Guid id,
        CancellationToken ct)
        where TEntity : Entity
        => _context.Set<TEntity>()
            .AsNoTracking()
            .FirstOrDefaultAsync(x => x.Id == id, ct);

    protected Task<TEntity?> GetTrackedByIdAsync<TEntity>(
        Guid id,
        CancellationToken ct)
        where TEntity : Entity
        => _context.Set<TEntity>()
            .FirstOrDefaultAsync(x => x.Id == id, ct);

    protected Task<bool> ExistsAsync<TEntity>(
        Guid id,
        CancellationToken ct = default)
        where TEntity : Entity
        => _context.Set<TEntity>()
            .AsNoTracking()
            .AnyAsync(e => e.Id == id, ct);

    private async Task SaveChangesAsync(
    CancellationToken ct = default)
    {
        try
        {
            await _context.SaveChangesAsync(ct);
        }
        catch (DbUpdateException ex)
        {
            foreach (var mapping in IndexMappings)
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