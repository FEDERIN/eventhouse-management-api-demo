using EventHouse.Management.Application.Common.Interfaces;
using EventHouse.Management.Application.Common.Pagination;
using EventHouse.Management.Application.Common.Sorting;
using EventHouse.Management.Application.Queries.EventVenueCalendars.GetAll;
using EventHouse.Management.Domain.Entities;
using EventHouse.Management.Domain.Enums;
using EventHouse.Management.Infrastructure.Persistence;
using EventHouse.Management.Infrastructure.Persistence.Exceptions;
using EventHouse.Management.Infrastructure.Persistence.Extensions;
using Microsoft.EntityFrameworkCore;

namespace EventHouse.Management.Infrastructure.Repositories;

internal class EventVenueCalendarRepository(ManagementDbContext context)
    : BaseRepository(context), IEventVenueCalendarRepository
{
    protected override IReadOnlyDictionary<string, UniqueConstraintMapping> IndexMappings
        => Empty;
    private static readonly IReadOnlyDictionary<string, UniqueConstraintMapping> Empty =
    new Dictionary<string, UniqueConstraintMapping>();

    #region WRITE
    public Task AddAsync(EventVenueCalendar entity, CancellationToken ct = default)
        => AddAsync<EventVenueCalendar>(entity, ct);

    public Task UpdateAsync(EventVenueCalendar entity, CancellationToken ct = default)
        => UpdateAsync<EventVenueCalendar>(entity, ct);

    public async Task SwapHeadlinerAsync(Guid calendarId, Guid oldArtistId, Guid newArtistId, CancellationToken ct = default)
    {
        using var transaction = await _context.Database.BeginTransactionAsync(ct);

        try
        {
            // 1. Remove headliner status from the previous artist
            int oldArtistRows = await _context.ArtistPerformances
                .Where(ap => ap.EventVenueCalendarId == calendarId && ap.ArtistId == oldArtistId)
                .ExecuteUpdateAsync(s => s.SetProperty(p => p.IsHeadliner, false), ct);

            // 2. Set the new artist as the headliner
            int newArtistRows = await _context.ArtistPerformances
                .Where(ap => ap.EventVenueCalendarId == calendarId && ap.ArtistId == newArtistId)
                .ExecuteUpdateAsync(s => s.SetProperty(p => p.IsHeadliner, true), ct);

            if (oldArtistRows == 0 || newArtistRows == 0)
                throw new InvalidOperationException("The exchange could not be completed: one of the artists is not assigned to this schedule.");

            await transaction.CommitAsync(ct);
        }
        catch
        {
            await transaction.RollbackAsync(ct); // ¡Aquí entrará tu test!
            throw;
        }
    }
    #endregion

    #region READ
    public Task<EventVenueCalendar?> GetByIdAsync(Guid id, CancellationToken ct = default)
    => GetByIdAsync<EventVenueCalendar>(id, ct);

    public async Task<EventVenueCalendar?> GetByIdWithPerformancesAsync(Guid id, CancellationToken ct = default)
    {
        return await _context.EventVenueCalendars
            .Include(c => c.Performances)
            .FirstOrDefaultAsync(c => c.Id == id, ct);
    }

    public async Task<PagedResultDto<EventVenueCalendar>> GetPagedAsync(EventVenueCalendarQueryCriteria criteria, CancellationToken ct = default)
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
            query = query.Where(ev => ev.TimeZoneId.Value == criteria.TimeZoneId);

        query = ApplyEventVenueCalendarSorting(query, criteria.SortBy, criteria.SortDirection);

        return await query.ToPagedResultAsync(criteria.Page, criteria.PageSize, ct);
    }
    #endregion

    #region VALIDATIONS

    public Task<bool> ExistsAsync(Guid id, CancellationToken ct = default)
        => ExistsAsync<EventVenueCalendar>(id, ct);

    public async Task<bool> IsSlotOccupiedAsync(Guid eventVenueId, DateTime startUtc, DateTime endUtc, Guid? excludeId = null, CancellationToken ct = default)
    {
        var targetVenueId = await _context.EventVenues
            .Where(ev => ev.Id == eventVenueId)
            .Select(ev => ev.VenueId)
            .FirstOrDefaultAsync(ct);

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
                ct);
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
            query.OrderByDirection(x => x.StartDate, sortDirection),

            EventVenueCalendarSortField.EndDate =>
                asc ? query.OrderBy(x => x.EndDate).ThenBy(x => x.StartDate)
                    : query.OrderByDescending(x => x.EndDate).ThenByDescending(x => x.StartDate),

            EventVenueCalendarSortField.TimeZoneId =>
                asc ? query.OrderBy(x => x.TimeZoneId.Value).ThenBy(x => x.StartDate)
                    : query.OrderByDescending(x => x.TimeZoneId.Value).ThenByDescending(x => x.StartDate),

            EventVenueCalendarSortField.Status =>
                asc ? query.OrderBy(x => x.Status).ThenBy(x => x.StartDate)
                    : query.OrderByDescending(x => x.Status).ThenByDescending(x => x.StartDate),

            _ => query.OrderByDirection(x => x.StartDate, sortDirection)
        };
    }
    #endregion
}
