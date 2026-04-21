
using EventHouse.Management.Application.Common.Pagination;
using EventHouse.Management.Application.Queries.EventVenueCalendars.GetAll;
using EventHouse.Management.Domain.Entities;

namespace EventHouse.Management.Application.Common.Interfaces
{
    public interface IEventVenueCalendarRepository
    {
        #region WRITE (Commands)
        Task AddAsync(EventVenueCalendar entity, CancellationToken cancellationToken = default);
        Task UpdateAsync(EventVenueCalendar entity, CancellationToken cancellationToken = default);
        #endregion

        #region READ (Queries)
        Task<EventVenueCalendar?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
        Task<EventVenueCalendar?> GetTrackedByIdAsync(Guid id, CancellationToken cancellationToken = default);
        Task<PagedResultDto<EventVenueCalendar>> GetPagedAsync(EventVenueCalendarQueryCriteria criteria, CancellationToken cancellationToken = default);
        #endregion

        #region VALIDATIONS
        Task<bool> IsSlotOccupiedAsync(
            Guid eventVenueId,
            DateTime startUtc,
            DateTime endUtc,
            Guid? excludeId = null,
            CancellationToken cancellationToken = default);
        #endregion
    }
}
