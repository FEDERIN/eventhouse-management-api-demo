using EventHouse.Management.Application.Common.Pagination;
using EventHouse.Management.Application.Queries.SeatingMaps.GetAll;
using EventHouse.Management.Domain.Entities;

namespace EventHouse.Management.Application.Common.Interfaces;

public interface ISeatingMapRepository
{
    #region WRITE
    Task AddAsync(SeatingMap entity, CancellationToken ct = default);
    Task UpdateAsync(SeatingMap entity, CancellationToken ct = default);
    #endregion

    #region READ
    Task<SeatingMap?> GetByIdAsync(Guid id, CancellationToken ct = default);
    Task<SeatingMap?> GetTrackedByIdAsync(Guid id, CancellationToken ct = default);
    Task<PagedResultDto<SeatingMap>> GetPagedAsync(
        SeatingMapQueryCriteria criteria,
        CancellationToken ct = default);

    Task<SeatingMap?> GetTrackedWithStructureByIdAsync(Guid id, CancellationToken ct = default);

    Task<SeatingMap?> GetWithStructureByIdAsync(
    Guid id,
    CancellationToken ct = default);

    #endregion

    #region VALIDATIONS
    Task<bool> ExistsAsync(Guid id, CancellationToken ct = default);
    #endregion
}
