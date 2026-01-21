using EventHouse.Management.Application.Common.Pagination;
using EventHouse.Management.Application.Queries.Events.GetAll;
using EventHouse.Management.Domain.Entities;

namespace EventHouse.Management.Application.Common.Interfaces;

public interface IEventRepository
{
    Task AddAsync(Event entity, CancellationToken cancellationToken = default);
    Task UpdateAsync(Event entity, CancellationToken cancellationToken = default);
    Task<bool> DeleteAsync(Guid id, CancellationToken cancellationToken = default);
    Task<Event?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
    Task<Event?> GetTrackedByIdAsync(Guid id, CancellationToken cancellationToken = default);
    Task<PagedResultDto<Event>> GetPagedAsync(
        EventQueryCriteria criteria,
        CancellationToken cancellationToken = default);
}
