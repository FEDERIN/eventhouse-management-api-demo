using EventHouse.Management.Application.Common.Pagination;
using EventHouse.Management.Application.Queries.ArtistPerformances.GetAll;
using EventHouse.Management.Domain.Entities;

namespace EventHouse.Management.Application.Common.Interfaces;

public interface IArtistPerformanceRepository
{
    #region READ
    Task<ArtistPerformance?> GetByIdAsync(Guid id, CancellationToken ct = default);
    Task<ArtistPerformance?> GetTrackedByIdAsync(Guid id, CancellationToken ct = default);
    Task<PagedResultDto<ArtistPerformance>> GetPagedAsync(
        ArtistPerformanceQueryCriteria criteria, CancellationToken ct = default);
    #endregion

    #region VALIDATION
    Task<bool> IsArtistBusyAsync(
        Guid artistId,
        Guid? currentPerformanceId,
        DateTime start,
        DateTime end,
        CancellationToken ct = default);
    #endregion
}
