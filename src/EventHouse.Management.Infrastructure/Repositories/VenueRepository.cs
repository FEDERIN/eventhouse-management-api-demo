using EventHouse.Management.Application.Common.Interfaces;
using EventHouse.Management.Application.Common.Pagination;
using EventHouse.Management.Application.Common.Sorting;
using EventHouse.Management.Application.Queries.Venues.GetAll;
using EventHouse.Management.Domain.Entities;
using EventHouse.Management.Infrastructure.Persistence;
using EventHouse.Management.Infrastructure.Persistence.Exceptions;
using EventHouse.Management.Infrastructure.Persistence.Extensions;
using Microsoft.EntityFrameworkCore;

namespace EventHouse.Management.Infrastructure.Repositories;
internal class VenueRepository(ManagementDbContext context) :
    BaseRepository(context), IVenueRepository
{
    protected override Dictionary<string, UniqueConstraintMapping> IndexMappings =>
    new()
    {
        ["UX_Venues_Name"] = new("VENUE_NAME_ALREADY_EXISTS", "The name already exists in another venue.")
    };

    public Task AddAsync(Venue entity, CancellationToken ct = default)
    => AddAsync<Venue>(entity, ct);

    public Task UpdateAsync(Venue entity, CancellationToken ct = default)
    => UpdateAsync<Venue>(entity, ct);

    public Task<bool> DeleteAsync(Guid id, CancellationToken ct = default)
        => DeleteAsync<Venue>(id, ct);

    public async Task<bool> ExistsAsync(Guid id, CancellationToken ct = default)
    {
        return await ExistsAsync<Venue>(id, ct);
    }

    public Task<Venue?> GetByIdAsync(Guid id, CancellationToken ct = default)
        => GetByIdAsync<Venue>(id, ct);

    public Task<Venue?> GetTrackedByIdAsync(Guid id, CancellationToken ct = default)
    => GetTrackedByIdAsync<Venue>(id, ct);

    public async Task<PagedResultDto<Venue>> GetPagedAsync(
        VenueQueryCriteria criteria,
        CancellationToken ct = default)
    {
        IQueryable<Venue> query = _context.Venues.AsNoTracking();

        if (!string.IsNullOrWhiteSpace(criteria.Name))
            query = query.Where(v => EF.Functions.Like(v.Name, $"%{criteria.Name}%"));
            
        if (!string.IsNullOrWhiteSpace(criteria.Address))
            query = query.Where(v => EF.Functions.Like(v.Address, $"%{criteria.Address}%"));

        if (!string.IsNullOrWhiteSpace(criteria.City))
            query = query.Where(v => v.City != null && v.City == criteria.City);

        if (!string.IsNullOrWhiteSpace(criteria.Region))
            query = query.Where(v => v.Region != null && v.Region == criteria.Region);

        if (!string.IsNullOrWhiteSpace(criteria.CountryCode))
            query = query.Where(v => v.CountryCode != null && v.CountryCode == criteria.CountryCode);

        if(criteria.Capacity.HasValue)
            query = query.Where(v => v.Capacity.HasValue && v.Capacity.Value >= criteria.Capacity.Value);

        if (criteria.IsActive is not null)
            query = query.Where(v => v.IsActive == criteria.IsActive.Value);

        query = ApplyVenueSorting(query, criteria.SortBy, criteria.SortDirection);

        return await query.ToPagedResultAsync(criteria.Page, criteria.PageSize, ct);
    }

    private static IQueryable<Venue> ApplyVenueSorting(
        IQueryable<Venue> query,
        VenueSortField? venueSortField,
        SortDirection sortDirection)
    {

        bool asc = sortDirection == SortDirection.Asc;

        query = venueSortField switch
        {
            VenueSortField.Name =>
                asc ? query.OrderBy(x => x.Name) : query.OrderByDescending(x => x.Name),

            VenueSortField.Address =>
                asc ? query.OrderBy(x => x.Address) : query.OrderByDescending(x => x.Address),

            VenueSortField.City =>
                asc ? query.OrderBy(x => x.City) : query.OrderByDescending(x => x.City),

            VenueSortField.Region =>
                asc ? query.OrderBy(x => x.Region) : query.OrderByDescending(x => x.Region),

            VenueSortField.CountryCode =>
                asc ? query.OrderBy(x => x.CountryCode) : query.OrderByDescending(x => x.CountryCode),

            VenueSortField.Capacity =>
                asc ? query.OrderBy(x => x.Capacity) : query.OrderByDescending(x => x.Capacity),

            VenueSortField.IsActive =>
                asc ? query.OrderBy(x => x.IsActive) : query.OrderByDescending(x => x.IsActive),

            _ => asc ? query.OrderBy(x => x.Name) : query.OrderByDescending(x => x.Name)
        };

        return query;
    }
}
