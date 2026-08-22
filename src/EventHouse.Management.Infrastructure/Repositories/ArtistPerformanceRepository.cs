using EventHouse.Management.Application.Common.Interfaces;
using EventHouse.Management.Application.Common.Pagination;
using EventHouse.Management.Application.Common.Sorting;
using EventHouse.Management.Application.Queries.ArtistPerformances.GetAll;
using EventHouse.Management.Domain.Entities;
using EventHouse.Management.Domain.Enums;
using EventHouse.Management.Infrastructure.Persistence;
using EventHouse.Management.Infrastructure.Persistence.Exceptions;
using EventHouse.Management.Infrastructure.Persistence.Extensions;
using Microsoft.EntityFrameworkCore;

namespace EventHouse.Management.Infrastructure.Repositories;

internal class ArtistPerformanceRepository(ManagementDbContext context) :
    BaseRepository(context), IArtistPerformanceRepository
{
    protected override IReadOnlyDictionary<string, UniqueConstraintMapping> IndexMappings
        => Empty;
    private static readonly IReadOnlyDictionary<string, UniqueConstraintMapping> Empty =
    new Dictionary<string, UniqueConstraintMapping>();


    #region READ
    public Task<ArtistPerformance?> GetByIdAsync(Guid id, CancellationToken ct = default)
    => GetByIdAsync<ArtistPerformance>(id, ct);

    public async Task<PagedResultDto<ArtistPerformance>> GetPagedAsync(
        ArtistPerformanceQueryCriteria criteria,
        CancellationToken ct = default)
    {
        var query = _context.ArtistPerformances
            .AsNoTracking()
            .Where(ap => ap.EventVenueCalendarId == criteria.EventVenueCalendarId);

        if (criteria.ArtistId.HasValue)
        {
            query = query.Where(ap => ap.ArtistId == criteria.ArtistId.Value);
        }

        if (criteria.IsHeadliner.HasValue)
        {
            query = query.Where(ap => ap.IsHeadliner == criteria.IsHeadliner.Value);
        }

        query = ApplyArtistPerformanceSorting(query, criteria.SortBy, criteria.SortDirection);

        return await query.ToPagedResultAsync(criteria.Page, criteria.PageSize, ct);
    }
    #endregion

    #region PRIVATE
    private static IQueryable<ArtistPerformance> ApplyArtistPerformanceSorting(
        IQueryable<ArtistPerformance> query,
        ArtistPerformanceSortField? sortBy,
        SortDirection sortDirection)
    {
        bool asc = sortDirection == SortDirection.Asc;

        return sortBy switch
        {
            ArtistPerformanceSortField.IsHeadliner =>
                asc ? query.OrderBy(x => x.IsHeadliner).ThenBy(x => x.SetStart)
                    : query.OrderByDescending(x => x.IsHeadliner).ThenByDescending(x => x.SetStart),

            ArtistPerformanceSortField.SetEnd =>
                asc ? query.OrderBy(x => x.SetEnd).ThenBy(x => x.SetStart)
                    : query.OrderByDescending(x => x.SetEnd).ThenByDescending(x => x.SetStart),

            ArtistPerformanceSortField.SetStart =>
                asc ? query.OrderBy(x => x.SetStart)
                    : query.OrderByDescending(x => x.SetStart),

            _ => asc ? query.OrderBy(x => x.SetStart)
                     : query.OrderByDescending(x => x.SetStart)
        };
    }
    #endregion

    #region VALIDATIONS
    /// <summary>
    /// Checks if the artist is busy in another published event.
    /// </summary>
    public async Task<bool> IsArtistBusyAsync(
        Guid artistId,
        Guid? currentPerformanceId,
        DateTime start,
        DateTime end,
        CancellationToken ct = default)
    {
        return await _context.ArtistPerformances
                .AnyAsync(ap =>
                    ap.ArtistId == artistId &&
                    ap.EventVenueCalendar != null &&
                    ap.EventVenueCalendar.Status == EventVenueCalendarStatus.Published &&
                    (currentPerformanceId == null || ap.Id != currentPerformanceId) &&
                    start < ap.SetEnd &&
                    ap.SetStart < end,
                    ct);
    }

    #endregion
}
