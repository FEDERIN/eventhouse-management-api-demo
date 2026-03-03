using EventHouse.Management.Application.Common.Interfaces;
using EventHouse.Management.Application.Common.Pagination;
using EventHouse.Management.Application.Common.Sorting;
using EventHouse.Management.Application.Queries.SeatingMaps.GetAll;
using EventHouse.Management.Domain.Entities;
using EventHouse.Management.Infrastructure.Persistence;
using EventHouse.Management.Infrastructure.Persistence.Extensions;
using Microsoft.EntityFrameworkCore;

namespace EventHouse.Management.Infrastructure.Repositories;

public class SeatingMapRepository(ManagementDbContext context) :
    BaseRepository(context), ISeatingMapRepository
{
    private static readonly Dictionary<string, (string? Code, string? Detail, bool ShouldIgnore)> IndexMappings = new()
    {
        { "SeatingMaps.Name", ("SEATING_MAP_ALREADY_EXISTS_IN_VENUE", "The name already exists in another seating map for the venue.", false) }
    };

    public async Task AddAsync(SeatingMap entity, CancellationToken cancellationToken = default)
    {
        await _context.SeatingMaps.AddAsync(entity, cancellationToken);
        await SaveChangesWithUniqueCheckAsync(IndexMappings, cancellationToken);

    }

    public async Task UpdateAsync(SeatingMap entity, CancellationToken cancellationToken = default)
    {
        if (_context.Entry(entity).State == EntityState.Detached)
            throw new InvalidOperationException("UpdateAsync requires a tracked entity. Use GetTrackedByIdAsync.");

        await SaveChangesWithUniqueCheckAsync(IndexMappings, cancellationToken);
    }

    public async Task<bool> DeleteAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var entity = await GetTrackedByIdAsync(id, cancellationToken);

        if (entity is null)
            return false;

        _context.SeatingMaps.Remove(entity);
        await _context.SaveChangesAsync(cancellationToken);

        return true;
    }

    public async Task<SeatingMap?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        return await _context.SeatingMaps.AsNoTracking().FirstOrDefaultAsync(e => e.Id == id, cancellationToken);
    }

    public async Task<SeatingMap?> GetTrackedByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        return await _context.SeatingMaps.FirstOrDefaultAsync(e => e.Id == id, cancellationToken);
    }

    public async Task<PagedResultDto<SeatingMap>> GetPagedAsync(SeatingMapQueryCriteria criteria, CancellationToken cancellationToken = default)
    {
        IQueryable<SeatingMap> query = _context.SeatingMaps.AsNoTracking();

        if (!string.IsNullOrWhiteSpace(criteria.Name))
            query = query.Where(v => EF.Functions.Like(v.Name, $"%{criteria.Name}%"));

        if (criteria.VenueId.HasValue)
            query = query.Where(v => v.VenueId == criteria.VenueId.Value);

        if (criteria.Version.HasValue)
            query = query.Where(v => v.Version == criteria.Version.Value);

        if (criteria.IsActive.HasValue)
            query = query.Where(v => v.IsActive == criteria.IsActive.Value);

        query = ApplySeatingMapSorting(query, criteria.SortBy, criteria.SortDirection);

        return await query.ToPagedResultAsync(criteria.Page, criteria.PageSize, cancellationToken);
    }

    private static IQueryable<SeatingMap> ApplySeatingMapSorting(IQueryable<SeatingMap> query,
            SeatingMapSortField? sortBy,
            SortDirection sortDirection)
    {
        bool asc = sortDirection == SortDirection.Asc;

        query = sortBy switch
        {
            SeatingMapSortField.Name =>
                asc ? query.OrderBy(x => x.Name) : query.OrderByDescending(x => x.Name),

            SeatingMapSortField.Version =>
                asc ? query.OrderBy(x => x.Version) : query.OrderByDescending(x => x.Version),

            SeatingMapSortField.IsActive =>
                asc ? query.OrderBy(x => x.IsActive) : query.OrderByDescending(x => x.IsActive),

            _ => asc ? query.OrderBy(x => x.Name) : query.OrderByDescending(x => x.Name)
        };
        return query;
    }
}
