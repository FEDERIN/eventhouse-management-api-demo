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
    internal class GenreRepository(ManagementDbContext context) : 
        BaseRepository(context), IGenreRepository
    {
        private static readonly Dictionary<string, (string? Code, string? Detail, bool ShouldIgnore)> IndexMappings = new()
        {
            { "Genres.Name", ("GENRE_NAME_ALREADY_EXISTS", "The name already exists in another genre.", false) }
        };

        public async Task AddAsync(Genre entity, CancellationToken cancellationToken = default)
        {
            await _context.Genres.AddAsync(entity, cancellationToken);
            await SaveChangesWithUniqueCheckAsync(IndexMappings, cancellationToken);
        }

        public async Task UpdateAsync(Genre entity, CancellationToken cancellationToken = default)
        {
            if (_context.Entry(entity).State == EntityState.Detached)
                throw new InvalidOperationException("UpdateAsync requires a tracked entity. Use GetTrackedByIdAsync.");

            await SaveChangesWithUniqueCheckAsync(IndexMappings, cancellationToken);
        }

        public async Task<bool> DeleteAsync(Guid id, CancellationToken cancellationToken = default)
        {
            var entity = await GetTrackedByIdAsync(id, cancellationToken);

            if (entity is null)
                return false;

            var artistGenre = await _context.ArtistGenres.FirstOrDefaultAsync(x => x.GenreId == id, cancellationToken: cancellationToken);

            if (artistGenre != null)
                throw new ConflictException(
                    code: "GENRE_HAS_ASSOCIATIONS",
                    title: "Genre cannot be deleted",
                    detail: "This genre cannot be deleted because it has associated entities."
                    );

            _context.Genres.Remove(entity);
            await _context.SaveChangesAsync(cancellationToken);

            return true;
        }

        public async Task<Genre?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
        {
            return await _context.Genres.AsNoTracking().FirstOrDefaultAsync(e => e.Id == id, cancellationToken);
        }

        public async Task<Genre?> GetTrackedByIdAsync(Guid id, CancellationToken cancellationToken = default)
        {
            return await _context.Genres.FirstOrDefaultAsync(e => e.Id == id, cancellationToken);
        }

        public async Task<PagedResultDto<Genre>> GetPagedAsync(GenreQueryCriteria criteria, CancellationToken cancellationToken = default)
        {
            IQueryable<Genre> query = _context.Genres.AsNoTracking();

            if (!string.IsNullOrWhiteSpace(criteria.Name))
                query = query.Where(g => EF.Functions.Like(g.Name, $"%{criteria.Name}%"));

            bool asc = criteria.SortDirection == SortDirection.Asc;

            query = criteria.SortBy switch
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
