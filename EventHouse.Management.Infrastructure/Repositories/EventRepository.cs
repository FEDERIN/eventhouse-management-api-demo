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

public class EventRepository(ManagementDbContext context) : IEventRepository
{
    private readonly ManagementDbContext _context = context;

    public async Task AddAsync(Event entity, CancellationToken cancellationToken = default)
    {
        await _context.Events.AddAsync(entity, cancellationToken);

        await SaveChangesAsync(entity, cancellationToken);
    }

    public async Task UpdateAsync(Event entity, CancellationToken cancellationToken = default)
    {
        _context.Events.Update(entity);

        await SaveChangesAsync(entity, cancellationToken);
    }

    public async Task<Event?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
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
            query = query.Where(v => v.Name == criteria.Name);

        if (!string.IsNullOrWhiteSpace(criteria.Description))
            query = query.Where(v => v.Description == criteria.Description);

        if (criteria.Scope.HasValue)
            query = query.Where(v => v.Scope == criteria.Scope.Value);


        var sortBy = criteria.SortBy ?? EventSortField.Name;
        bool asc = criteria.SortDirection == SortDirection.Asc;

        query = sortBy switch
        {
            EventSortField.Name =>
                asc ? query.OrderBy(x => x.Name) : query.OrderByDescending(x => x.Name),

            EventSortField.Description =>
                asc ? query.OrderBy(x => x.Description) : query.OrderByDescending(x => x.Description),

            EventSortField.Scope =>
                asc ? query.OrderBy(x => x.Scope) : query.OrderByDescending(x => x.Scope),

            _ => asc ? query.OrderBy(x => x.Name) : query.OrderByDescending(x => x.Name)
        };

        return await query.ToPagedResultAsync(criteria.Page, criteria.PageSize, cancellationToken);
    }
    public async Task<bool> ExistsAsync(Guid id, CancellationToken cancellationToken = default)
    {
        return await _context.Events
            .AnyAsync(e => e.Id == id, cancellationToken);
    }

    public async Task<bool> DeleteAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var entity = await GetByIdAsync(id, cancellationToken);
        if (entity is null) 
            return false;

        _context.Events.Remove(entity);
        await _context.SaveChangesAsync(cancellationToken);

        return true;
    }

    private async Task SaveChangesAsync(Event entity, CancellationToken cancellationToken)
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
