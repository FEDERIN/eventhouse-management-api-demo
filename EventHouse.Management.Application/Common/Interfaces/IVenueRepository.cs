using EventHouse.Management.Application.Common.Pagination;
using EventHouse.Management.Application.Queries.Venues.GetAll;
using EventHouse.Management.Domain.Entities;

namespace EventHouse.Management.Application.Common.Interfaces
{
    public interface IVenueRepository
    {
        Task AddAsync(Venue entity, CancellationToken cancellationToken = default);
        Task UpdateAsync(Venue entity, CancellationToken cancellationToken = default);
        Task<Venue?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
        Task<PagedResultDto<Venue>> GetPagedAsync(
            VenueQueryCriteria criteria,
            CancellationToken cancellationToken = default);
        Task<bool> ExistsAsync(Guid id, CancellationToken cancellationToken = default);
        Task<bool> ExistsByNameAsync(string name, CancellationToken cancellationToken = default);
        Task<bool> DeleteAsync(Guid id, CancellationToken cancellationToken = default);
    }
}
