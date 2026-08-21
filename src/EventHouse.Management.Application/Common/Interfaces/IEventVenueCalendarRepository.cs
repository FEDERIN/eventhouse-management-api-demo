
using EventHouse.Management.Application.Common.Pagination;
using EventHouse.Management.Application.Queries.EventVenueCalendars.GetAll;
using EventHouse.Management.Domain.Entities;

namespace EventHouse.Management.Application.Common.Interfaces;

public interface IEventVenueCalendarRepository
{
    #region WRITE
    Task AddAsync(EventVenueCalendar entity, CancellationToken ct = default);
    Task UpdateAsync(EventVenueCalendar entity, CancellationToken ct = default);
    Task SwapHeadlinerAsync(Guid eventVenueCalendar, Guid oldArtistId, Guid newArtistId, CancellationToken ct = default);
    #endregion

    #region READ
    Task<EventVenueCalendar?> GetByIdAsync(Guid id, CancellationToken ct = default);
    Task<EventVenueCalendar?> GetByIdWithPerformancesAsync(Guid id, CancellationToken ct = default);
    Task<PagedResultDto<EventVenueCalendar>> GetPagedAsync(EventVenueCalendarQueryCriteria criteria, CancellationToken ct = default);
    #endregion

    #region VALIDATIONS
    Task<bool> ExistsAsync(Guid id, CancellationToken ct = default);

    Task<bool> IsSlotOccupiedAsync(
        Guid eventVenueId,
        DateTime startUtc,
        DateTime endUtc,
        Guid? excludeId = null,
        CancellationToken ct = default);
    #endregion
}
