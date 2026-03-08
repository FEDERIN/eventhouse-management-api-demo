using EventHouse.Management.Application.Common.Interfaces;
using EventHouse.Management.Application.Common.Pagination;
using EventHouse.Management.Application.Common.Sorting;
using EventHouse.Management.Application.Queries.EventVenues.GetAll;
using EventHouse.Management.Domain.Entities;
using EventHouse.Management.Infrastructure.Persistence;
using EventHouse.Management.Infrastructure.Persistence.Extensions;
using Microsoft.EntityFrameworkCore;

namespace EventHouse.Management.Infrastructure.Repositories;

internal class EventVenueRepository(ManagementDbContext context) :
    BaseRepository(context), IEventVenueRepository
{
    private static readonly Dictionary<string, (string? Code, string? Detail, bool ShouldIgnore)> EventVenueMappings = new()
    {
        { "EventVenues.EventId, EventVenues.VenueId", ("EVENT_ALREADY_ASSIGNED", "This event is already assigned to this venue.", false) }
    };

    #region WRITE (Commands)
    public async Task AddAsync(EventVenue entity, CancellationToken cancellationToken = default)
    {
        await _context.EventVenues.AddAsync(entity, cancellationToken);
        await SaveChangesWithUniqueCheckAsync(EventVenueMappings, cancellationToken);
    }

    public async Task UpdateAsync(EventVenue entity, CancellationToken cancellationToken = default)
    {
        if (_context.Entry(entity).State == EntityState.Detached)
            throw new InvalidOperationException("UpdateAsync requires a tracked entity. Use GetTrackedByIdAsync.");

        await SaveChangesWithUniqueCheckAsync(EventVenueMappings, cancellationToken);
    }
    #endregion

    #region READ (Queries)
    public async Task<EventVenue?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        return await _context.EventVenues
            .AsNoTracking()
            .Include(ev => ev.Event)
            .Include(ev => ev.Venue)
            .FirstOrDefaultAsync(e => e.Id == id, cancellationToken);
    }

    public async Task<EventVenue?> GetTrackedByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        return await _context.EventVenues.FirstOrDefaultAsync(e => e.Id == id, cancellationToken);
    }

    public async Task<PagedResultDto<EventVenue>> GetPagedAsync(
        EventVenueQueryCriteria criteria,
        CancellationToken cancellationToken)
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
            cancellationToken
        );
    }
    #endregion
}
