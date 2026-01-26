using EventHouse.Management.Application.Common.Pagination;
using EventHouse.Management.Application.Queries.Artists.GetAll;
using EventHouse.Management.Domain.Entities;

namespace EventHouse.Management.Application.Common.Interfaces;

public interface IArtistRepository
{
    Task AddAsync(Artist entity, CancellationToken cancellationToken = default);
    Task UpdateAsync(Artist entity, CancellationToken cancellationToken = default);
    Task SetPrimaryGenreAsync(Guid artistId, Guid genreOldId, Guid genreId, CancellationToken ct);
    Task<bool> DeleteAsync(Guid id, CancellationToken cancellationToken = default);
    Task<Artist?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
    Task<Artist?> GetTrackedByIdAsync(Guid id, CancellationToken cancellationToken = default);
    Task<PagedResultDto<Artist>> GetPagedAsync(
        ArtistQueryCriteria criteria,
        CancellationToken cancellationToken = default);
}
