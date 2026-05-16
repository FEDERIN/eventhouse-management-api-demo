using EventHouse.Management.Application.Common.Interfaces;
using EventHouse.Management.Application.Common.Pagination;
using EventHouse.Management.Application.Common.Sorting;
using EventHouse.Management.Application.Exceptions;
using EventHouse.Management.Application.Queries.Artists.GetAll;
using EventHouse.Management.Domain.Entities;
using EventHouse.Management.Infrastructure.Persistence;
using EventHouse.Management.Infrastructure.Persistence.Extensions;
using Microsoft.EntityFrameworkCore;

namespace EventHouse.Management.Infrastructure.Repositories;

internal class ArtistRepository(ManagementDbContext context) :
    BaseRepository(context), IArtistRepository
{
    private static readonly Dictionary<string, (string? Code, string? Detail, bool ShouldIgnore)> ArtistMappings = new()
    {
        { "UX_Artists_Name", ("ARTIST_NAME_ALREADY_EXISTS", "Artist name already exists.", false) },
        { "UX_ArtistGenres_Artist_Genre", (null, null, true) }
    };

    #region WRITE

    public async Task AddAsync(Artist entity, CancellationToken cancellationToken = default)
    {
        await _context.Artists.AddAsync(entity, cancellationToken);
        await SaveChangesWithUniqueCheckAsync(ArtistMappings, cancellationToken);
    }

    public async Task UpdateAsync(Artist entity, CancellationToken cancellationToken = default)
    {
        if (_context.Entry(entity).State == EntityState.Detached)
            throw new InvalidOperationException("UpdateAsync requires a tracked entity. Use GetTrackedByIdAsync.");

        await SaveChangesWithUniqueCheckAsync(ArtistMappings, cancellationToken);
    }

    public async Task SetPrimaryGenreAsync(Guid artistId, Guid genreOldId, Guid genreId, CancellationToken ct)
    {
        // Execute everything inside a single transaction to ensure consistency
        using var transaction = await _context.Database.BeginTransactionAsync(ct);

        try
        {
            // 1. Reset the old primary genre if it exists
            if (genreOldId != Guid.Empty)
            {
                await _context.ArtistGenres
                    .Where(ag => ag.ArtistId == artistId && ag.GenreId == genreOldId)
                    .ExecuteUpdateAsync(setters => setters.SetProperty(ag => ag.IsPrimary, false), ct);
            }

            // 2. Set the new primary genre
            await _context.ArtistGenres
                .Where(ag => ag.ArtistId == artistId && ag.GenreId == genreId)
                .ExecuteUpdateAsync(setters => setters.SetProperty(ag => ag.IsPrimary, true), ct);

            // Commit all changes at once
            await transaction.CommitAsync(ct);
        }
        catch
        {
            await transaction.RollbackAsync(ct);
            throw;
        }
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

    #endregion

    #region READ
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
    #endregion

    #region VALIDATIONS
    public async Task<bool> ExistsAsync(Guid id, CancellationToken cancellationToken = default)
    {
        return await _context.Artists.AnyAsync(a => a.Id == id, cancellationToken);
    }
    #endregion
}
