using EventHouse.Management.Application.Common.Interfaces;
using EventHouse.Management.Application.Common.Pagination;
using EventHouse.Management.Application.Common.Sorting;
using EventHouse.Management.Application.Queries.SeatingMaps.GetAll;
using EventHouse.Management.Domain.Entities;
using EventHouse.Management.Infrastructure.Persistence;
using EventHouse.Management.Infrastructure.Persistence.Exceptions;
using EventHouse.Management.Infrastructure.Persistence.Extensions;
using Microsoft.EntityFrameworkCore;

namespace EventHouse.Management.Infrastructure.Repositories;

internal class SeatingMapRepository(ManagementDbContext context) :
    BaseRepository(context), ISeatingMapRepository
{
    protected override Dictionary<string, UniqueConstraintMapping> IndexMappings =>
    new()
    {
        ["UX_SeatingMap_Venue_Name_Version"] = new("SEATING_MAP_ALREADY_EXISTS_IN_VENUE", "The name and version already exists in another seating map for the venue.")
    };

    #region WRITE
    public Task AddAsync(SeatingMap entity, CancellationToken ct = default)
        => AddAsync<SeatingMap>(entity, ct);

    public async Task UpdateAsync(SeatingMap entity, CancellationToken ct = default)
        => await UpdateAsync<SeatingMap>(entity, ct);

    public Task<bool> DeleteAsync(Guid id, CancellationToken ct = default)
        => DeleteAsync<SeatingMap>(id, ct);
    #endregion

    #region READ
    public Task<SeatingMap?> GetByIdAsync(Guid id, CancellationToken ct = default)
    => GetByIdAsync<SeatingMap>(id, ct);

    public Task<SeatingMap?> GetTrackedByIdAsync(Guid id, CancellationToken ct = default)
        => GetTrackedByIdAsync<SeatingMap>(id, ct);

    public async Task<PagedResultDto<SeatingMap>> GetPagedAsync(SeatingMapQueryCriteria criteria, CancellationToken ct = default)
    {
        IQueryable<SeatingMap> query = _context.SeatingMaps.AsNoTracking();

        if (!string.IsNullOrWhiteSpace(criteria.Name))
            query = query.Where(v => EF.Functions.Like(v.Name, $"%{criteria.Name}%"));

        if (criteria.VenueId.HasValue)
            query = query.Where(v => v.VenueId == criteria.VenueId.Value);

        if (criteria.IsActive.HasValue)
            query = query.Where(v => v.IsActive == criteria.IsActive.Value);

        query = ApplySeatingMapSorting(query, criteria.SortBy, criteria.SortDirection);

        return await query.ToPagedResultAsync(criteria.Page, criteria.PageSize, ct);
    }
    #endregion

    #region VALIDATIONS
    public Task<bool> ExistsAsync(Guid id, CancellationToken ct = default)
        => ExistsAsync<SeatingMap>(id, ct);

    #endregion

    #region PRIVATE
    private static IQueryable<SeatingMap> ApplySeatingMapSorting(IQueryable<SeatingMap> query,
            SeatingMapSortField? sortBy,
            SortDirection sortDirection)
    {
        bool asc = sortDirection == SortDirection.Asc;

        query = sortBy switch
        {
            SeatingMapSortField.Name =>
            query.OrderByDirection(x => x.Name, sortDirection),

            SeatingMapSortField.Version =>
            query.OrderByDirection(x => x.Version, sortDirection),

            SeatingMapSortField.IsActive =>
                asc ? query.OrderBy(x => x.IsActive).ThenBy(x => x.Name) : query.OrderByDescending(x => x.IsActive).ThenBy(x => x.Name),

            _ => query.OrderByDirection(x => x.Name, sortDirection),

        };
        return query;
    }
    #endregion
}
