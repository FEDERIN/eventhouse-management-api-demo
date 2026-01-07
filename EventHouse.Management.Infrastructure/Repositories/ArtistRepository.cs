using EventHouse.Management.Application.Common.Interfaces;
using EventHouse.Management.Application.Common.Pagination;
using EventHouse.Management.Application.Common.Sorting;
using EventHouse.Management.Application.Queries.Artists.GetAll;
using EventHouse.Management.Domain.Entities;
using EventHouse.Management.Infrastructure.Persistence;
using EventHouse.Management.Infrastructure.Persistence.Extensions;
using Microsoft.EntityFrameworkCore;

namespace EventHouse.Management.Infrastructure.Repositories;

public class ArtistRepository(ManagementDbContext context) : IArtistRepository
{
    private readonly ManagementDbContext _context = context;

    public async Task AddAsync(Artist entity, CancellationToken cancellationToken = default)
    {
        await _context.Artists.AddAsync(entity, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);
    }

    public async Task<bool> DeleteAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var entity = await _context.Artists.FindAsync([id], cancellationToken);

        if(entity is null)
            return false;

        _context.Artists.Remove(entity);
        await _context.SaveChangesAsync(cancellationToken);

        return true;
    }

    public async Task<bool> ExistsAsync(Guid id, CancellationToken cancellationToken = default)
    {
        return await _context.Artists.AnyAsync(e => e.Id == id, cancellationToken);
    }

    public async Task<IEnumerable<Artist>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        return await _context.Artists.ToListAsync(cancellationToken);
    }

    public async Task<Artist?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        return await _context.Artists.FirstOrDefaultAsync(e => e.Id == id, cancellationToken);
    }

    public async Task<PagedResultDto<Artist>> GetPagedAsync(ArtistQueryCriteria criteria, CancellationToken cancellationToken = default)
    {
        IQueryable<Artist> query = _context.Artists.AsNoTracking();

        if (!string.IsNullOrWhiteSpace(criteria.Name))
            query = query.Where(a => a.Name == criteria.Name);

        if (criteria.Category.HasValue)
            query = query.Where(a => a.Category == criteria.Category.Value);

        var sortBy = criteria.SortBy ?? ArtistSortField.Name;
        bool asc = criteria.SortDirection == SortDirection.Asc;

        query = sortBy switch
        {
            ArtistSortField.Name =>
                asc ? query.OrderBy(x => x.Name) : query.OrderByDescending(x => x.Name),

            ArtistSortField.Category =>
                asc ? query.OrderBy(x => x.Category) : query.OrderByDescending(x => x.Category),

            _ => asc ? query.OrderBy(x => x.Name) : query.OrderByDescending(x => x.Name)
        };

        return await query.ToPagedResultAsync(criteria.Page, criteria.PageSize, cancellationToken);
    }

    public async Task UpdateAsync(Artist entity, CancellationToken cancellationToken = default)
    {
        _context.Artists.Update(entity);
        await _context.SaveChangesAsync(cancellationToken);
    }
}
