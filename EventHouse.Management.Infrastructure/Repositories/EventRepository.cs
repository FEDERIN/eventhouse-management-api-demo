using EventHouse.Management.Application.Common.Interfaces;
using EventHouse.Management.Application.Common.Pagination;
using EventHouse.Management.Application.Common.Sorting;
using EventHouse.Management.Application.Exceptions;
using EventHouse.Management.Application.Queries.Events.GetAll;
using EventHouse.Management.Domain.Entities;
using EventHouse.Management.Infrastructure.Persistence;
using EventHouse.Management.Infrastructure.Persistence.Exceptions;
using EventHouse.Management.Infrastructure.Persistence.Extensions;
using Microsoft.EntityFrameworkCore;


namespace EventHouse.Management.Infrastructure.Repositories;

internal class EventRepository(ManagementDbContext context) : IEventRepository
{
    private readonly ManagementDbContext _context = context;

    public async Task AddAsync(Event entity, CancellationToken cancellationToken = default)
    {
        await _context.Events.AddAsync(entity, cancellationToken);

        await SaveChangesWithUniqueCheckAsync(entity, cancellationToken);
    }

    public async Task UpdateAsync(Event entity, CancellationToken cancellationToken = default)
    {
        if (_context.Entry(entity).State == EntityState.Detached)
            throw new InvalidOperationException("UpdateAsync requires a tracked entity. Use GetTrackedByIdAsync.");

        await SaveChangesWithUniqueCheckAsync(entity, cancellationToken);
    }

    public async Task<bool> DeleteAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var entity = await GetTrackedByIdAsync(id, cancellationToken);

        if (entity is null)
            return false;

        _context.Events.Remove(entity);

        await _context.SaveChangesAsync(cancellationToken);

        return true;
    }

    public async Task<Event?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        return await _context.Events
            .AsNoTracking()
            .FirstOrDefaultAsync(e => e.Id == id, cancellationToken);
    }

    public async Task<Event?> GetTrackedByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        return await _context.Events
            .FirstOrDefaultAsync(e => e.Id == id, cancellationToken);
    }

    public async Task<PagedResultDto<Event>> GetPagedAsync(
        EventQueryCriteria criteria,
        CancellationToken cancellationToken = default)
    {
        IQueryable<Event> query = _context.Events.AsNoTracking();

        if (!string.IsNullOrWhiteSpace(criteria.Name))
            query = query.Where(e => EF.Functions.Like(e.Name, $"%{criteria.Name}%"));

        if (!string.IsNullOrWhiteSpace(criteria.Description))
            query = query.Where(e => e.Description != null &&
                                     EF.Functions.Like(e.Description, $"%{criteria.Description}%"));

        if (criteria.Scope.HasValue)
            query = query.Where(e => e.Scope == criteria.Scope.Value);


        var sortBy = criteria.SortBy ?? EventSortField.Name;
        bool asc = criteria.SortDirection == SortDirection.Asc;

        query = sortBy switch
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

        return await query.ToPagedResultAsync(criteria.Page, criteria.PageSize, cancellationToken);
    }

    private async Task SaveChangesWithUniqueCheckAsync(Event entity, CancellationToken cancellationToken)
    {
        try
        {
            await _context.SaveChangesAsync(cancellationToken);
        }
        catch (DbUpdateException ex) when (ex.IsUniqueViolation())
        {
            throw new ConflictException(
                code: "EVENT_NAME_ALREADY_EXISTS",
                title: "Unique constraint violated",
                detail: $"Event with name '{entity.Name}' already exists."
            );
        }
    }
}
