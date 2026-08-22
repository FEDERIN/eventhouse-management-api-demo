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
    public async Task AddAsync(Genre entity, CancellationToken ct = default)
    {
        await innerRepository.AddAsync(entity, ct);
        await cacheService.InvalidateByTagAsync("genres", ct);
    }

    public async Task UpdateAsync(Genre entity, CancellationToken ct = default)
    {
        await innerRepository.UpdateAsync(entity, ct);
        await cacheService.RemoveAsync($"{CachePrefix}{entity.Id}", ct);
        await cacheService.InvalidateByTagAsync("genres", ct);
    }

    public async Task<bool> DeleteAsync(Guid id, CancellationToken ct = default)
    {
        var deleted = await innerRepository.DeleteAsync(id, ct);
        if (deleted)
        {
            await cacheService.RemoveAsync($"{CachePrefix}{id}", ct);
            await cacheService.InvalidateByTagAsync("genres", ct);
        }
        return deleted;
    }
    #endregion

    #region WRITE
    public async Task<Genre?> GetByIdAsync(Guid id, CancellationToken ct = default)
    {
        return await cacheService.GetOrAddAsync(
            key: $"{CachePrefix}{id}",
            factory: ct => innerRepository.GetByIdAsync(id, ct),
            expiration: TimeSpan.FromMinutes(30),
            tags: ["genres"],
            ct: ct);
    }

    public async Task<Genre?> GetTrackedByIdAsync(Guid id, CancellationToken ct = default)
    {
        return await innerRepository.GetTrackedByIdAsync(id, ct);
    }

    public async Task<PagedResultDto<Genre>> GetPagedAsync(GenreQueryCriteria criteria, CancellationToken ct = default)
    {
        return await innerRepository.GetPagedAsync(criteria, ct);
    }
    #endregion
}
