using EventHouse.Management.Application.Common.Interfaces;
using EventHouse.Management.Application.Common.Pagination;
using EventHouse.Management.Application.Common.Sorting;
using EventHouse.Management.Application.Exceptions;
using EventHouse.Management.Application.Queries.Genres.GetAll;
using EventHouse.Management.Domain.Entities;
using EventHouse.Management.Infrastructure.Persistence;
using EventHouse.Management.Infrastructure.Persistence.Exceptions;
using EventHouse.Management.Infrastructure.Persistence.Extensions;
using Microsoft.EntityFrameworkCore;

namespace EventHouse.Management.Infrastructure.Repositories
{
    internal class GenreRepository(ManagementDbContext context) : IGenreRepository
    {
        private readonly ManagementDbContext _context = context;

        public async Task AddAsync(Genre entity, CancellationToken cancellationToken = default)
        {
            await _context.Genres.AddAsync(entity, cancellationToken);
            await SaveChangesWithUniqueCheckAsync(entity, cancellationToken);
        }

        public async Task UpdateAsync(Genre entity, CancellationToken cancellationToken = default)
        {
            _context.Genres.Update(entity);

            await SaveChangesWithUniqueCheckAsync(entity, cancellationToken);
        }

        public async Task<bool> DeleteAsync(Guid id, CancellationToken cancellationToken = default)
        {
            var entity = await GetTrackedByIdAsync(id, cancellationToken);

            if (entity is null)
                return false;

            _context.Genres.Remove(entity);
            await _context.SaveChangesAsync(cancellationToken);

            return true;
        }

        public async Task<Genre?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
        {
            return await _context.Genres.FirstOrDefaultAsync(e => e.Id == id, cancellationToken);
        }

        public async Task<Genre?> GetTrackedByIdAsync(Guid id, CancellationToken cancellationToken = default)
        {
            return await _context.Genres.AsNoTracking().FirstOrDefaultAsync(e => e.Id == id, cancellationToken);
        }

        public async Task<PagedResultDto<Genre>> GetPagedAsync(GenreQueryCriteria criteria, CancellationToken cancellationToken = default)
        {
            IQueryable<Genre> query = _context.Genres.AsNoTracking();

            if (!string.IsNullOrWhiteSpace(criteria.Name))
                query = query.Where(g => EF.Functions.Like(g.Name, $"%{criteria.Name}%"));

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

        private async Task SaveChangesWithUniqueCheckAsync(Genre entity, CancellationToken cancellationToken)
        {
            try
            {
                await _context.SaveChangesAsync(cancellationToken);
            }
            catch (DbUpdateException ex) when (ex.IsUniqueViolation())
            {
                throw new ConflictException(
                    code: "GENRE_NAME_ALREADY_EXISTS",
                    title: "Unique constraint violated",
                    detail: $"Genre with name '{entity.Name}' already exists."
                );
            }
        }

    }
}
