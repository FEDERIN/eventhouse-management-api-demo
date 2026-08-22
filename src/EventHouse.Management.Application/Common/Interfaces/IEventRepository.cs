using EventHouse.Management.Application.Common.Pagination;
using EventHouse.Management.Application.Queries.Events.GetAll;
using EventHouse.Management.Domain.Entities;

namespace EventHouse.Management.Application.Common.Interfaces;

public interface IEventRepository
{
    Task AddAsync(Event entity, CancellationToken ct = default);
    Task UpdateAsync(Event entity, CancellationToken ct = default);
    Task<bool> DeleteAsync(Guid id, CancellationToken ct = default);
    Task<Event?> GetByIdAsync(Guid id, CancellationToken ct = default);
    Task<Event?> GetTrackedByIdAsync(Guid id, CancellationToken ct = default);
    Task<PagedResultDto<Event>> GetPagedAsync(
        EventQueryCriteria criteria,
        CancellationToken ct = default);
}
