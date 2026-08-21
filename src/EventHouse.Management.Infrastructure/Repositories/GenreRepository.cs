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
        protected override Dictionary<string, UniqueConstraintMapping> IndexMappings =>
        new()
        {
            ["UX_Genres_Name"] = new(
                "GENRE_NAME_ALREADY_EXISTS",
                "The name already exists in another genre.",
                false)
        };

        #region WRITE
        public Task AddAsync(Genre entity, CancellationToken ct = default)
            => AddAsync<Genre>(entity, ct);

        public Task UpdateAsync(Genre entity, CancellationToken ct = default) 
            => UpdateAsync<Genre>(entity, ct);

        public async Task<bool> DeleteAsync(Guid id, CancellationToken ct = default)
        {
            var entity = await GetTrackedByIdAsync(id, ct);

            if (entity is null)
                return false;

            var artistGenre = await _context.ArtistGenres.FirstOrDefaultAsync(x => x.GenreId == id, ct);

            if (artistGenre != null)
                throw new ConflictException(
                    code: "GENRE_HAS_ASSOCIATIONS",
                    title: "Genre cannot be deleted",
                    detail: "This genre cannot be deleted because it has associated entities."
                    );

            _context.Genres.Remove(entity);
            await _context.SaveChangesAsync(ct);

            return true;
        }
        #endregion

        #region READ
        public Task<Genre?> GetByIdAsync(Guid id, CancellationToken ct = default)
        => GetByIdAsync<Genre>(id, ct);

        public Task<Genre?> GetTrackedByIdAsync(Guid id, CancellationToken ct = default)
            => GetTrackedByIdAsync<Genre>(id, ct);

        public async Task<PagedResultDto<Genre>> GetPagedAsync(GenreQueryCriteria criteria, CancellationToken ct = default)
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

            return await query.ToPagedResultAsync(criteria.Page, criteria.PageSize, ct);
        }
        #endregion
    }
}
