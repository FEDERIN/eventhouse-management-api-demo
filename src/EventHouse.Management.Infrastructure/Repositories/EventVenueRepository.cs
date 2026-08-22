using EventHouse.Management.Application.Common.Interfaces;
using EventHouse.Management.Application.Common.Pagination;
using EventHouse.Management.Application.Common.Sorting;
using EventHouse.Management.Application.Queries.EventVenues.GetAll;
using EventHouse.Management.Domain.Entities;
using EventHouse.Management.Infrastructure.Persistence;
using EventHouse.Management.Infrastructure.Persistence.Exceptions;
using EventHouse.Management.Infrastructure.Persistence.Extensions;
using Microsoft.EntityFrameworkCore;

namespace EventHouse.Management.Infrastructure.Repositories;

internal class EventVenueRepository(ManagementDbContext context) :
    BaseRepository(context), IEventVenueRepository
{
    protected override Dictionary<string, UniqueConstraintMapping> IndexMappings =>
    new()
    {
        ["UX_EventVenues_Event_Venue"] = new(
            "EVENT_ALREADY_ASSIGNED",
            "This event is already assigned to this venue.")
    };

    #region WRITE
    public Task AddAsync(EventVenue entity, CancellationToken ct = default)
        => AddAsync<EventVenue>(entity, ct);

    public Task UpdateAsync(EventVenue entity, CancellationToken ct = default)
        => UpdateAsync<EventVenue>(entity, ct);
    #endregion

    #region READ
    public async Task<EventVenue?> GetByIdAsync(Guid id, CancellationToken ct = default)
    {
        return await _context.EventVenues
            .AsNoTracking()
            .Include(ev => ev.Event)
            .Include(ev => ev.Venue)
            .FirstOrDefaultAsync(e => e.Id == id, ct);
    }

    public Task<EventVenue?> GetTrackedByIdAsync(Guid id, CancellationToken ct = default)
        => GetTrackedByIdAsync<EventVenue>(id, ct);

    public async Task<PagedResultDto<EventVenue>> GetPagedAsync(
        EventVenueQueryCriteria criteria,
        CancellationToken ct)
    {
        IQueryable<EventVenue> query = _context.EventVenues.AsNoTracking();

        if (!criteria.EventId.HasValue)
            query = query.Include(ev => ev.Event);

        if (!criteria.VenueId.HasValue)
            query = query.Include(ev => ev.Venue);

        if (criteria.EventId.HasValue)
            query = query.Where(ev => ev.EventId == criteria.EventId.Value);

        if (criteria.VenueId.HasValue)
            query = query.Where(ev => ev.VenueId == criteria.VenueId.Value);

        if (criteria.Status.HasValue)
            query = query.Where(ev => ev.Status == criteria.Status.Value);

        bool asc = criteria.SortDirection == SortDirection.Asc;

        query = criteria.SortBy switch
        {
            EventVenueSortField.Status => asc
                ? query.OrderBy(x => x.Status).ThenBy(x => x.Id)
                : query.OrderByDescending(x => x.Status).ThenBy(x => x.Id),
            _ => asc ? query.OrderBy(x => x.Status).ThenBy(x => x.Id) : query.OrderByDescending(x => x.Status).ThenBy(x => x.Id)
        };

        return await query.ToPagedResultAsync(
            criteria.Page,
            criteria.PageSize,
            ct
        );
    }
    #endregion


    #region VALIDATIONS
    public Task<bool> ExistsAsync(Guid id, CancellationToken ct = default)
        => ExistsAsync<EventVenue>(id, ct);
    
    #endregion
}
