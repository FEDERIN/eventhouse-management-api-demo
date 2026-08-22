using EventHouse.Management.Application.Common.Pagination;
using EventHouse.Management.Application.Queries.EventVenues.GetAll;
using EventHouse.Management.Domain.Entities;

namespace EventHouse.Management.Application.Common.Interfaces;

/// <summary>
/// Repository contract for EventVenue persistence, following ISO/IEC 25010 
/// maintainability and analyzability standards.
/// </summary>
public interface IEventVenueRepository
{
    #region WRITE
    Task AddAsync(EventVenue entity, CancellationToken ct = default);

    Task UpdateAsync(EventVenue entity, CancellationToken ct = default);
    #endregion

    #region READ
    Task<EventVenue?> GetByIdAsync(Guid id, CancellationToken ct = default);

    Task<EventVenue?> GetTrackedByIdAsync(Guid id, CancellationToken ct = default);

    Task<PagedResultDto<EventVenue>> GetPagedAsync(
        EventVenueQueryCriteria criteria,
        CancellationToken ct
    );
    #endregion

    #region VALIDATIONS
    Task<bool> ExistsAsync(Guid id, CancellationToken ct = default);
    #endregion
}