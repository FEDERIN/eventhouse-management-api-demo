using EventHouse.Management.Application.Common.Interfaces;
using EventHouse.Management.Application.Common.Pagination;
using EventHouse.Management.Application.Common.Sorting;
using EventHouse.Management.Application.Queries.Genres.GetAll;
using EventHouse.Management.Domain.Entities;
using EventHouse.Management.Infrastructure.Persistence;
using EventHouse.Management.Infrastructure.Persistence.Extensions;
using Microsoft.EntityFrameworkCore;

namespace EventHouse.Management.Infrastructure.Repositories
{
    public class GenreRepository(ManagementDbContext context) : IGenreRepository
    {
        private readonly ManagementDbContext _context = context;

        public async Task AddAsync(Genre entity, CancellationToken cancellationToken = default)
        {
            await _context.Genres.AddAsync(entity, cancellationToken);
            await _context.SaveChangesAsync(cancellationToken);
        }

        public async Task<Genre?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
        {
            return await _context.Genres.FirstOrDefaultAsync(e => e.Id == id, cancellationToken);
        }

        public async Task<bool> ExistsAsync(Guid id, CancellationToken cancellationToken = default)
        {
            return await _context.Genres.AnyAsync(e => e.Id == id, cancellationToken);
        }

        public async Task<bool> DeleteAsync(Guid id, CancellationToken cancellationToken = default)
        {
            var entity = await _context.Genres.FindAsync([id], cancellationToken);

            if(entity is null)
                return false;

            _context.Genres.Remove(entity);
            await _context.SaveChangesAsync(cancellationToken);

            return true;
        }

        public async Task UpdateAsync(Genre entity, CancellationToken cancellationToken = default)
        {
            _context.Genres.Update(entity);
            await _context.SaveChangesAsync(cancellationToken);
        }

        public async Task<bool> ExistsByNameAsync(string name, CancellationToken cancellationToken = default)
        {
            var normalized = name.Trim();
            return await _context.Genres
                .AnyAsync(g => g.Name == normalized, cancellationToken);
        }

        public async Task<PagedResultDto<Genre>> GetPagedAsync(GenreQueryCriteria criteria, CancellationToken cancellationToken = default)
        {
            IQueryable<Genre> query = _context.Genres.AsNoTracking();

            if (!string.IsNullOrWhiteSpace(criteria.Name))
                query = query.Where(ev => ev.Name == criteria.Name);

            var sortBy = criteria.SortBy ?? GenreSortField.Name;
            bool asc = criteria.SortDirection == SortDirection.Asc;

            query = sortBy switch
            {
                GenreSortField.Name =>
                    asc ? query.OrderBy(x => x.Name) : query.OrderByDescending(x => x.Name),

                _ => asc ? query.OrderBy(x => x.Name) : query.OrderByDescending(x => x.Name)
            };

            return await query.ToPagedResultAsync(criteria.Page, criteria.PageSize, cancellationToken);
        }
    }
}
