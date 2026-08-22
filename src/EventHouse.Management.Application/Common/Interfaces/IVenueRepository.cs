using EventHouse.Management.Application.Common.Pagination;
using EventHouse.Management.Application.Queries.Venues.GetAll;
using EventHouse.Management.Domain.Entities;

namespace EventHouse.Management.Application.Common.Interfaces;

public interface IVenueRepository
{
    Task AddAsync(Venue entity, CancellationToken ct = default);
    Task UpdateAsync(Venue entity, CancellationToken ct = default);
    Task<bool> DeleteAsync(Guid id, CancellationToken ct = default);
    Task<bool> ExistsAsync(Guid id, CancellationToken ct = default);
    Task<Venue?> GetByIdAsync(Guid id, CancellationToken ct = default);
    Task<Venue?> GetTrackedByIdAsync(Guid id, CancellationToken ct = default);
    Task<PagedResultDto<Venue>> GetPagedAsync(
        VenueQueryCriteria criteria,
        CancellationToken ct = default);
}
