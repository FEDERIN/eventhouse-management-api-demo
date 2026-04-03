using EventHouse.Management.Application.Common.Pagination;
using EventHouse.Management.Application.Queries.SeatingMaps.GetAll;
using EventHouse.Management.Domain.Entities;

namespace EventHouse.Management.Application.Common.Interfaces;

public interface ISeatingMapRepository
{
    Task AddAsync(SeatingMap entity, CancellationToken cancellationToken = default);
    Task UpdateAsync(SeatingMap entity, CancellationToken cancellationToken = default);
    Task<bool> DeleteAsync(Guid id, CancellationToken cancellationToken = default);
    Task<SeatingMap?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
    Task<SeatingMap?> GetTrackedByIdAsync(Guid id, CancellationToken cancellationToken = default);
    Task<PagedResultDto<SeatingMap>> GetPagedAsync(
        SeatingMapQueryCriteria criteria,
        CancellationToken cancellationToken = default);
}
