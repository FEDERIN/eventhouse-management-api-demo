using Core.Cache.Abstractions;
using EventHouse.Management.Application.Common.Interfaces;
using EventHouse.Management.Application.Common.Pagination;
using EventHouse.Management.Application.Queries.Genres.GetAll;
using EventHouse.Management.Domain.Entities;

namespace EventHouse.Management.Infrastructure.Repositories;

internal class CachedGenreRepository(
        IGenreRepository innerRepository,
        ICoreCache cacheService

    ) : IGenreRepository
{
    private const string CachePrefix = "genres:";

    #region WRITE
    public async Task AddAsync(Genre entity, CancellationToken cancellationToken = default)
    {
        await innerRepository.AddAsync(entity, cancellationToken);
        await cacheService.InvalidateByTagAsync("genres", cancellationToken);
    }

    public async Task UpdateAsync(Genre entity, CancellationToken cancellationToken = default)
    {
        await innerRepository.UpdateAsync(entity, cancellationToken);
        await cacheService.RemoveAsync($"{CachePrefix}{entity.Id}", cancellationToken);
        await cacheService.InvalidateByTagAsync("genres", cancellationToken);
    }

    public async Task<bool> DeleteAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var deleted = await innerRepository.DeleteAsync(id, cancellationToken);
        if (deleted)
        {
            await cacheService.RemoveAsync($"{CachePrefix}{id}", cancellationToken);
            await cacheService.InvalidateByTagAsync("genres", cancellationToken);
        }
        return deleted;
    }
    #endregion

    #region WRITE
    public async Task<Genre?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        return await cacheService.GetOrAddAsync(
            key: $"{CachePrefix}{id}",
            factory: ct => innerRepository.GetByIdAsync(id, ct),
            expiration: TimeSpan.FromMinutes(30),
            tags: ["genres"],
            ct: cancellationToken);
    }

    public async Task<Genre?> GetTrackedByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        return await innerRepository.GetTrackedByIdAsync(id, cancellationToken);
    }

    public async Task<PagedResultDto<Genre>> GetPagedAsync(GenreQueryCriteria criteria, CancellationToken cancellationToken = default)
    {
        return await innerRepository.GetPagedAsync(criteria, cancellationToken);
    }
    #endregion
}
