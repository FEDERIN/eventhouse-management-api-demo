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

    public async Task<SeatingMap?> GetTrackedWithStructureByIdAsync(
        Guid id,
        CancellationToken ct = default)
    {
        return await _context.SeatingMaps
            .Include(x => x.Sections)
            .ThenInclude(x => x.Rows)
            .ThenInclude(x => x.Seats)
            .FirstOrDefaultAsync(x => x.Id == id, ct);
    }

    public async Task<SeatingMap?> GetWithStructureByIdAsync(
    Guid id,
    CancellationToken ct = default)
    {
        return await _context.SeatingMaps
            .AsNoTracking()
            .Include(x => x.Sections)
            .ThenInclude(x => x.Rows)
            .ThenInclude(x => x.Seats)
            .FirstOrDefaultAsync(x => x.Id == id, ct);
    }

    #endregion

    #region VALIDATIONS
    public Task<bool> ExistsAsync(Guid id, CancellationToken ct = default)
        => ExistsAsync<SeatingMap>(id, ct);

    #endregion

    #region PRIVATE
    private static IQueryable<SeatingMap> ApplySeatingMapSorting(
        IQueryable<SeatingMap> query,
        SeatingMapSortField? sortBy,
        SortDirection sortDirection)
    {
        return sortBy switch
        {
            SeatingMapSortField.Name =>
                query.OrderByDirection(
                    x => x.Name,
                    sortDirection),

            SeatingMapSortField.Version =>
                query.OrderByDirection(
                    x => x.Version,
                    sortDirection),

            SeatingMapSortField.IsActive =>
                query
                    .OrderByDirection(
                        x => x.IsActive,
                        sortDirection)
                    .ThenByDirection(
                        x => x.Name,
                        sortDirection),

            _ =>
                query.OrderByDirection(
                    x => x.Name,
                    sortDirection)
        };
    }
    #endregion
}
