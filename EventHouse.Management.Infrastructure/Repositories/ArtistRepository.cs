using EventHouse.Management.Application.Common.Interfaces;
using EventHouse.Management.Application.Common.Pagination;
using EventHouse.Management.Application.Common.Sorting;
using EventHouse.Management.Application.Exceptions;
using EventHouse.Management.Application.Queries.Artists.GetAll;
using EventHouse.Management.Domain.Entities;
using EventHouse.Management.Infrastructure.Persistence;
using EventHouse.Management.Infrastructure.Persistence.Exceptions;
using EventHouse.Management.Infrastructure.Persistence.Extensions;
using Microsoft.EntityFrameworkCore;

namespace EventHouse.Management.Infrastructure.Repositories;

internal class ArtistRepository(ManagementDbContext context) : IArtistRepository
{
    private readonly ManagementDbContext _context = context;

    public async Task AddAsync(Artist entity, CancellationToken cancellationToken = default)
    {
        await _context.Artists.AddAsync(entity, cancellationToken);
        await SaveChangesWithUniqueCheckAsync(entity, cancellationToken);
    }

    public async Task UpdateAsync(Artist entity, CancellationToken cancellationToken = default)
    {
        if (_context.Entry(entity).State == EntityState.Detached)
            throw new InvalidOperationException("UpdateAsync requires a tracked entity. Use GetTrackedByIdAsync.");

        await SaveChangesWithUniqueCheckAsync(entity, cancellationToken);
    }

    public async Task SetPrimaryGenreAsync(Guid artistId, Guid genreOldId, Guid genreId, CancellationToken ct)
    {
        if (!genreOldId.Equals(Guid.Empty))
        {
            await _context.Database.ExecuteSqlInterpolatedAsync($"""
                UPDATE ArtistGenres
                SET IsPrimary = 0
                WHERE ArtistId = {artistId} AND GenreId = {genreOldId}
            """, ct);

            await _context.SaveChangesAsync(ct);
        }

        await _context.Database.ExecuteSqlInterpolatedAsync($"""
                UPDATE ArtistGenres
                SET IsPrimary = 1
                WHERE ArtistId = {artistId} AND GenreId = {genreId}
            """, ct);

        await _context.SaveChangesAsync(ct);
    }

    public async Task<bool> DeleteAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var entity = await GetTrackedByIdAsync(id, cancellationToken);

        if (entity is null)
            return false;

        if (entity.Genres.Count != 0)
            throw new ConflictException(
                code: "ARTIST_HAS_ASSOCIATIONS",
                title: "Artist cannot be deleted",
                detail: "This artist cannot be deleted because it has associated entities."
                );

        _context.Artists.Remove(entity);

        await _context.SaveChangesAsync(cancellationToken);

        return true;
    }

    public async Task<Artist?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        return await _context.Artists
            .Include(i => i.Genres)
            .AsNoTracking().FirstOrDefaultAsync(e => e.Id == id, cancellationToken);
    }

    public Task<Artist?> GetTrackedByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        return _context.Artists
            .Include(i => i.Genres)
            .FirstOrDefaultAsync(e => e.Id == id, cancellationToken);
    }

    public async Task<PagedResultDto<Artist>> GetPagedAsync(ArtistQueryCriteria criteria, CancellationToken cancellationToken = default)
    {
        IQueryable<Artist> query = 
            _context.Artists
            .Include(i => i.Genres)
            .AsNoTracking();

        if (!string.IsNullOrWhiteSpace(criteria.Name))
            query = query.Where(a => EF.Functions.Like(a.Name, $"%{criteria.Name}%"));

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

    private async Task SaveChangesWithUniqueCheckAsync(Artist entity, CancellationToken cancellationToken)
    {
        try
        {
            await _context.SaveChangesAsync(cancellationToken);
        }
        catch (DbUpdateException ex) when (ex.IsUniqueViolation("IX_Artists_Name"))
        {
            throw new ConflictException(
                code: "ARTIST_NAME_ALREADY_EXISTS",
                title: "Unique constraint violated",
                detail: $"Artist with name '{entity.Name}' already exists."
            );
        }
        catch (DbUpdateException ex) when (ex.IsUniqueViolation("IX_ArtistGenres_ArtistId_GenreId"))
        {
            _context.ChangeTracker.Clear();
            return;
        }
    }
}
