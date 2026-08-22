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

internal class ArtistRepository(ManagementDbContext context) :
    BaseRepository(context), IArtistRepository
{
    protected override Dictionary<string, UniqueConstraintMapping> IndexMappings =>
    new()
    {
        ["UX_Artists_Name"] = new(
            "ARTIST_NAME_ALREADY_EXISTS",
            "Artist name already exists.",
            false),
        ["UX_ArtistGenres_Artist_Genre"] = new(
            null,
            null,
            true)
    };

    #region WRITE
    public Task AddAsync(Artist entity, CancellationToken ct = default)
    => AddAsync<Artist>(entity, ct);

    public Task UpdateAsync(Artist entity, CancellationToken ct = default)
        => UpdateAsync<Artist>(entity, ct);

    public async Task SetPrimaryGenreAsync(Guid artistId, Guid genreOldId, Guid genreId, CancellationToken ct = default)
    {
        using var transaction = await _context.Database.BeginTransactionAsync(ct);

        try
        {
            if (genreOldId != Guid.Empty)
            {
                await _context.ArtistGenres
                    .Where(ag => ag.ArtistId == artistId && ag.GenreId == genreOldId)
                    .ExecuteUpdateAsync(setters => setters.SetProperty(ag => ag.IsPrimary, false), ct);
            }

            int affectedRows = await _context.ArtistGenres
                .Where(ag => ag.ArtistId == artistId && ag.GenreId == genreId)
                .ExecuteUpdateAsync(setters => setters.SetProperty(ag => ag.IsPrimary, true), ct);

            if (affectedRows == 0)
            {
                throw new InvalidOperationException($"The primary genre could not be established. The genre {genreId} is not associated with the artist {artistId}.");
            }

            await transaction.CommitAsync(ct);
        }
        catch
        {
            await transaction.RollbackAsync(ct);
            throw;
        }
    }

    public async Task<bool> DeleteAsync(Guid id, CancellationToken ct = default)
    {
        var entity = await GetTrackedByIdAsync(id, ct);

        if (entity is null)
            return false;

        if (entity.Genres.Count != 0)
            throw new ConflictException(
                code: "ARTIST_HAS_ASSOCIATIONS",
                title: "Artist cannot be deleted",
                detail: "This artist cannot be deleted because it has associated entities."
                );

        _context.Artists.Remove(entity);

        await _context.SaveChangesAsync(ct);

        return true;
    }

    #endregion

    #region READ
    public async Task<Artist?> GetByIdAsync(Guid id, CancellationToken ct = default)
    {
        return await _context.Artists
            .Include(i => i.Genres)
            .AsNoTracking().FirstOrDefaultAsync(e => e.Id == id, ct);
    }

    public Task<Artist?> GetTrackedByIdAsync(Guid id, CancellationToken ct = default)
    {
        return _context.Artists
            .Include(i => i.Genres)
            .FirstOrDefaultAsync(e => e.Id == id, ct);
    }

    public async Task<PagedResultDto<Artist>> GetPagedAsync(ArtistQueryCriteria criteria, CancellationToken ct = default)
    {
        IQueryable<Artist> query = 
            _context.Artists
            .Include(i => i.Genres)
            .AsNoTracking();

        if (!string.IsNullOrWhiteSpace(criteria.Name))
            query = query.Where(a => EF.Functions.Like(a.Name, $"%{criteria.Name}%"));

        if (criteria.Category.HasValue)
            query = query.Where(a => a.Category == criteria.Category.Value);

        bool asc = criteria.SortDirection == SortDirection.Asc;

        query = criteria.SortBy switch
        {
            ArtistSortField.Name =>
                asc ? query.OrderBy(x => x.Name) : query.OrderByDescending(x => x.Name),

            ArtistSortField.Category =>
                asc ? query.OrderBy(x => x.Category) : query.OrderByDescending(x => x.Category),

            _ => asc ? query.OrderBy(x => x.Name) : query.OrderByDescending(x => x.Name)
        };

        return await query.ToPagedResultAsync(criteria.Page, criteria.PageSize, ct);
    }
    #endregion

    #region VALIDATIONS
    public Task<bool> ExistsAsync(Guid id, CancellationToken ct = default)
        => ExistsAsync<Artist>(id, ct);

    
    #endregion
}
