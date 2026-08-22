using EventHouse.Management.Application.Common.Pagination;
using EventHouse.Management.Application.Queries.Genres.GetAll;
using EventHouse.Management.Domain.Entities;

namespace EventHouse.Management.Application.Common.Interfaces
{
    public interface IGenreRepository
    {
        #region WRITE
        Task AddAsync(Genre entity, CancellationToken ct = default);
        Task UpdateAsync(Genre entity, CancellationToken ct = default);
        Task<bool> DeleteAsync(Guid id, CancellationToken ct = default);
        #endregion

        #region WRITE
        Task<Genre?> GetByIdAsync(Guid id, CancellationToken ct = default);
        Task<Genre?> GetTrackedByIdAsync(Guid id, CancellationToken ct = default);
        Task<PagedResultDto<Genre>> GetPagedAsync(
            GenreQueryCriteria criteria,
            CancellationToken ct = default);
        #endregion
    }
}
