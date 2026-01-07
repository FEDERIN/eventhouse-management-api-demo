using EventHouse.Management.Application.Common.Pagination;
using EventHouse.Management.Application.Queries.Genres.GetAll;
using EventHouse.Management.Domain.Entities;

namespace EventHouse.Management.Application.Common.Interfaces
{
    public interface IGenreRepository
    {
        Task AddAsync(Genre entity, CancellationToken cancellationToken = default);
        Task UpdateAsync(Genre entity, CancellationToken cancellationToken = default);
        Task<Genre?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
        Task<PagedResultDto<Genre>> GetPagedAsync(
            GenreQueryCriteria criteria,
            CancellationToken cancellationToken = default);
        Task<bool> ExistsAsync(Guid id, CancellationToken cancellationToken = default);
        Task<bool> ExistsByNameAsync(string name, CancellationToken cancellationToken = default);
        Task<bool> DeleteAsync(Guid id, CancellationToken cancellationToken = default);
    }
}
