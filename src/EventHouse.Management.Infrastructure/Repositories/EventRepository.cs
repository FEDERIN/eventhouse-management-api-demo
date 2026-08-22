using EventHouse.Management.Application.Common.Interfaces;
using EventHouse.Management.Application.Common.Pagination;
using EventHouse.Management.Application.Common.Sorting;
using EventHouse.Management.Application.Queries.Events.GetAll;
using EventHouse.Management.Domain.Entities;
using EventHouse.Management.Infrastructure.Persistence;
using EventHouse.Management.Infrastructure.Persistence.Exceptions;
using EventHouse.Management.Infrastructure.Persistence.Extensions;
using Microsoft.EntityFrameworkCore;


namespace EventHouse.Management.Infrastructure.Repositories;

internal class EventRepository(ManagementDbContext context) :
    BaseRepository(context), IEventRepository
{
    protected override Dictionary<string, UniqueConstraintMapping> IndexMappings =>
    new()
    {
        ["UX_Event_Name"] = new(
            "EVENT_NAME_ALREADY_EXISTS",
            "The name already exists in another event.",
            false)
    };

    public Task AddAsync(Event entity, CancellationToken ct = default)
        => AddAsync<Event>(entity, ct);

    public Task UpdateAsync(Event entity, CancellationToken ct = default)
        => UpdateAsync<Event>(entity, ct);

    public Task<bool> DeleteAsync(Guid id, CancellationToken ct = default)
        => DeleteAsync<Event>(id, ct);

    public Task<Event?> GetByIdAsync(Guid id, CancellationToken ct = default)
    => GetByIdAsync<Event>(id, ct);

    public Task<Event?> GetTrackedByIdAsync(Guid id, CancellationToken ct = default)
        => GetTrackedByIdAsync<Event>(id, ct);

    public async Task<PagedResultDto<Event>> GetPagedAsync(
        EventQueryCriteria criteria,
        CancellationToken ct = default)
    {
        IQueryable<Event> query = _context.Events.AsNoTracking();

        if (!string.IsNullOrWhiteSpace(criteria.Name))
            query = query.Where(e => EF.Functions.Like(e.Name, $"%{criteria.Name}%"));

        if (!string.IsNullOrWhiteSpace(criteria.Description))
            query = query.Where(e => e.Description != null &&
                                     EF.Functions.Like(e.Description, $"%{criteria.Description}%"));

        if (criteria.Scope.HasValue)
            query = query.Where(e => e.Scope == criteria.Scope.Value);

        bool asc = criteria.SortDirection == SortDirection.Asc;

        query = criteria.SortBy switch
        {
            EventSortField.Name =>
                asc ? query.OrderBy(x => x.Name) : query.OrderByDescending(x => x.Name),

            EventSortField.Description =>
                asc
                  ? query.OrderBy(e => e.Description == null).ThenBy(e => e.Description)
                  : query.OrderBy(e => e.Description == null).ThenByDescending(e => e.Description),

            EventSortField.Scope =>
                asc ? query.OrderBy(x => x.Scope) : query.OrderByDescending(x => x.Scope),

            _ => asc ? query.OrderBy(x => x.Name) : query.OrderByDescending(x => x.Name)
        };

        return await query.ToPagedResultAsync(criteria.Page, criteria.PageSize, ct);
    }
}
