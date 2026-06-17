
using EventHouse.Management.Application.Common.Pagination;
using EventHouse.Management.Application.Queries.EventVenueCalendars.GetAll;
using EventHouse.Management.Domain.Entities;

namespace EventHouse.Management.Application.Common.Interfaces;

public interface IEventVenueCalendarRepository
{
    #region WRITE
    Task AddAsync(EventVenueCalendar entity, CancellationToken cancellationToken = default);
    Task UpdateAsync(EventVenueCalendar entity, CancellationToken cancellationToken = default);
    Task SwapHeadlinerAsync(Guid eventVenueCalendar, Guid oldArtistId, Guid newArtistId, CancellationToken ct);
    #endregion

    #region READ
    Task<EventVenueCalendar?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
    Task<EventVenueCalendar?> GetByIdWithPerformancesAsync(Guid id, CancellationToken ct);
    Task<PagedResultDto<EventVenueCalendar>> GetPagedAsync(EventVenueCalendarQueryCriteria criteria, CancellationToken cancellationToken = default);
    #endregion

    #region VALIDATIONS
    Task<bool> ExistsAsync(Guid id, CancellationToken cancellationToken = default);

    Task<bool> IsSlotOccupiedAsync(
        Guid eventVenueId,
        DateTime startUtc,
        DateTime endUtc,
        Guid? excludeId = null,
        CancellationToken cancellationToken = default);
    #endregion
}
