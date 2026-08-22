using EventHouse.Management.Application.Common.Pagination;
using EventHouse.Management.Application.Queries.Artists.GetAll;
using EventHouse.Management.Domain.Entities;

namespace EventHouse.Management.Application.Common.Interfaces;

public interface IArtistRepository
{
    #region WRITE
    Task AddAsync(Artist entity, CancellationToken ct = default);
    Task UpdateAsync(Artist entity, CancellationToken ct = default);
    Task SetPrimaryGenreAsync(Guid artistId, Guid genreOldId, Guid genreId, CancellationToken ct = default);
    Task<bool> DeleteAsync(Guid id, CancellationToken ct = default);
    #endregion

    #region READ
    Task<Artist?> GetByIdAsync(Guid id, CancellationToken ct = default);
    Task<Artist?> GetTrackedByIdAsync(Guid id, CancellationToken ct = default);
    Task<PagedResultDto<Artist>> GetPagedAsync(
        ArtistQueryCriteria criteria,
        CancellationToken ct = default);
    #endregion

    #region VALIDATIONS
    Task<bool> ExistsAsync(Guid id, CancellationToken ct = default);
    #endregion
}
