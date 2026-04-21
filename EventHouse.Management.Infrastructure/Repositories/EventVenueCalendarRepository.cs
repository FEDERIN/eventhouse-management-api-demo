using EventHouse.Management.Application.Common.Interfaces;
using EventHouse.Management.Application.Common.Pagination;
using EventHouse.Management.Application.Common.Sorting;
using EventHouse.Management.Application.Queries.EventVenueCalendars.GetAll;
using EventHouse.Management.Domain.Entities;
using EventHouse.Management.Domain.Enums;
using EventHouse.Management.Infrastructure.Persistence;
using EventHouse.Management.Infrastructure.Persistence.Extensions;
using Microsoft.EntityFrameworkCore;

namespace EventHouse.Management.Infrastructure.Repositories;

public class EventVenueCalendarRepository(ManagementDbContext context) 
    : BaseRepository(context), IEventVenueCalendarRepository
{

    private static readonly Dictionary<string, (string? Code, string? Detail, bool ShouldIgnore)> EventVenueCalendarMappings = [];

    #region WRITE
    public async Task AddAsync(EventVenueCalendar entity, CancellationToken cancellationToken = default)
    {
        await _context.EventVenueCalendars.AddAsync(entity, cancellationToken);
        await SaveChangesWithUniqueCheckAsync(EventVenueCalendarMappings, cancellationToken);
    }
    public async Task UpdateAsync(EventVenueCalendar entity, CancellationToken cancellationToken = default)
    {
        if (_context.Entry(entity).State == EntityState.Detached)
            throw new InvalidOperationException("UpdateAsync requires a tracked entity. Use GetTrackedByIdAsync.");

        await SaveChangesWithUniqueCheckAsync(EventVenueCalendarMappings, cancellationToken);
    }
    #endregion

    #region READ
    public async Task<EventVenueCalendar?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        return await _context.EventVenueCalendars
            .AsNoTracking()
            .FirstOrDefaultAsync(e => e.Id == id, cancellationToken);
    }

    public async Task<EventVenueCalendar?> GetTrackedByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        return await _context.EventVenueCalendars
            .FirstOrDefaultAsync(e => e.Id == id, cancellationToken);
    }

    public async Task<PagedResultDto<EventVenueCalendar>> GetPagedAsync(EventVenueCalendarQueryCriteria criteria, CancellationToken cancellationToken = default)
    {
        IQueryable<EventVenueCalendar> query = _context.EventVenueCalendars.AsNoTracking();

        if (criteria.EventVenueId.HasValue)
            query = query.Where(ev => ev.EventVenueId == criteria.EventVenueId.Value);

        if (criteria.SeatingMapId.HasValue)
            query = query.Where(ev => ev.SeatingMapId == criteria.SeatingMapId.Value);

        if (criteria.StartDate.HasValue)
            query = query.Where(ev => ev.StartDate >= criteria.StartDate.Value);

        if (criteria.EndDate.HasValue)
            query = query.Where(ev => ev.EndDate <= criteria.EndDate.Value);

        if (criteria.Status.HasValue)
            query = query.Where(ev => ev.Status == criteria.Status.Value);

        if (!string.IsNullOrEmpty(criteria.TimeZoneId))
            query = query.Where(ev => ev.TimeZoneId == criteria.TimeZoneId);

        query = ApplyEventVenueCalendarSorting(query, criteria.SortBy, criteria.SortDirection);

        return await query.ToPagedResultAsync(criteria.Page, criteria.PageSize, cancellationToken);
    }
    #endregion

    #region VALIDATIONS
    public async Task<bool> IsSlotOccupiedAsync(Guid eventVenueId, DateTime startUtc, DateTime endUtc, Guid? excludeId = null, CancellationToken cancellationToken = default)
    {
        var targetVenueId = await _context.EventVenues
            .Where(ev => ev.Id == eventVenueId)
            .Select(ev => ev.VenueId)
            .FirstOrDefaultAsync(cancellationToken);

        if (targetVenueId == Guid.Empty) return false;

        return await _context.EventVenueCalendars
            .AsNoTracking()
            .AnyAsync(c =>
                c.EventVenue != null &&
                c.EventVenue.VenueId == targetVenueId &&
                c.EventVenue.Status == EventVenueStatus.Active &&
                c.Id != excludeId &&
                c.Status != EventVenueCalendarStatus.Cancelled &&
                c.StartDate < endUtc &&
                c.EndDate > startUtc,
                cancellationToken);
    }
    #endregion

    #region PRIVATE
    private static IQueryable<EventVenueCalendar> ApplyEventVenueCalendarSorting(
        IQueryable<EventVenueCalendar> query,
        EventVenueCalendarSortField? sortBy,
        SortDirection sortDirection)
    {
        bool asc = sortDirection == SortDirection.Asc;

        return sortBy switch
        {
            EventVenueCalendarSortField.StartDate =>
                asc ? query.OrderBy(x => x.StartDate)
                    : query.OrderByDescending(x => x.StartDate),

            EventVenueCalendarSortField.EndDate =>
                asc ? query.OrderBy(x => x.EndDate).ThenBy(x => x.StartDate)
                    : query.OrderByDescending(x => x.EndDate).ThenByDescending(x => x.StartDate),

            EventVenueCalendarSortField.TimeZoneId =>
                asc ? query.OrderBy(x => x.TimeZoneId).ThenBy(x => x.StartDate)
                    : query.OrderByDescending(x => x.TimeZoneId).ThenByDescending(x => x.StartDate),

            EventVenueCalendarSortField.Status =>
                asc ? query.OrderBy(x => x.Status).ThenBy(x => x.StartDate)
                    : query.OrderByDescending(x => x.Status).ThenByDescending(x => x.StartDate),

            _ => asc ? query.OrderBy(x => x.StartDate)
                     : query.OrderByDescending(x => x.StartDate)
        };
    }
    #endregion
}
