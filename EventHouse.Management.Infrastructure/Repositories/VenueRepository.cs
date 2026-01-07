using EventHouse.Management.Application.Common.Interfaces;
using EventHouse.Management.Application.Common.Pagination;
using EventHouse.Management.Application.Common.Sorting;
using EventHouse.Management.Application.Queries.Venues.GetAll;
using EventHouse.Management.Domain.Entities;
using EventHouse.Management.Infrastructure.Persistence;
using EventHouse.Management.Infrastructure.Persistence.Extensions;
using Microsoft.EntityFrameworkCore;

namespace EventHouse.Management.Infrastructure.Repositories
{
    public class VenueRepository(ManagementDbContext context) : IVenueRepository
    {
        private readonly ManagementDbContext _context = context;

        public async Task AddAsync(Venue entity, CancellationToken cancellationToken = default)
        {
            await _context.Venues.AddAsync(entity, cancellationToken);
            await _context.SaveChangesAsync(cancellationToken);
        }

        public async Task<Venue?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
        {
            return await _context.Venues.FirstOrDefaultAsync(e => e.Id == id, cancellationToken);
        }

        public async Task<bool> ExistsAsync(Guid id, CancellationToken cancellationToken = default)
        {
            return await _context.Venues.AnyAsync(e => e.Id == id, cancellationToken);
        }

        public async Task<bool> DeleteAsync(Guid id, CancellationToken cancellationToken = default)
        {
            var entity = await _context.Venues.FindAsync([id], cancellationToken);

            if (entity is null)
                return false;

            _context.Venues.Remove(entity);
            await _context.SaveChangesAsync(cancellationToken);

            return true;
        }

        public async Task UpdateAsync(Venue entity, CancellationToken cancellationToken = default)
        {
            _context.Venues.Update(entity);
            await _context.SaveChangesAsync(cancellationToken);
        }

        public async Task<bool> ExistsByNameAsync(string name, CancellationToken cancellationToken = default)
        {
            var normalized = name.Trim();
            return await _context.Venues
                .AnyAsync(g => g.Name == normalized, cancellationToken);
        }

        public async Task<PagedResultDto<Venue>> GetPagedAsync(
            VenueQueryCriteria criteria,
            CancellationToken cancellationToken = default)
        {
            IQueryable<Venue> query = _context.Venues.AsNoTracking();

            if (!string.IsNullOrWhiteSpace(criteria.Name))
                query = query.Where(v => v.Address != null && v.Name.IndexOf(criteria.Name) > 0);

            if (!string.IsNullOrWhiteSpace(criteria.Address))
                query = query.Where(v => v.Address != null && v.Address == criteria.Address);

            if (!string.IsNullOrWhiteSpace(criteria.City))
                query = query.Where(v => v.City != null && v.City == criteria.City);

            if (!string.IsNullOrWhiteSpace(criteria.Region))
                query = query.Where(v => v.Region != null && v.Region == criteria.Region);

            if (!string.IsNullOrWhiteSpace(criteria.CountryCode))
                query = query.Where(v => v.CountryCode != null && v.CountryCode == criteria.CountryCode);

            if (criteria.IsActive is not null)
                query = query.Where(v => v.IsActive == criteria.IsActive.Value);

            query = ApplyVenueSorting(query, criteria.SortBy, criteria.SortDirection);

            return await query.ToPagedResultAsync(criteria.Page, criteria.PageSize, cancellationToken);
        }

        private static IQueryable<Venue> ApplyVenueSorting(
            IQueryable<Venue> query,
            VenueSortField? venueSortField,
            SortDirection sortDirection)
        {

            var sortBy = venueSortField ?? VenueSortField.Name;
            bool asc = sortDirection == SortDirection.Asc;

            query = sortBy switch
            {
                VenueSortField.Name =>
                    asc ? query.OrderBy(x => x.Name) : query.OrderByDescending(x => x.Name),

                VenueSortField.Address =>
                    asc ? query.OrderBy(x => x.Address) : query.OrderByDescending(x => x.Address),

                VenueSortField.City =>
                    asc ? query.OrderBy(x => x.City) : query.OrderByDescending(x => x.City),

                VenueSortField.Region =>
                    asc ? query.OrderBy(x => x.Region) : query.OrderByDescending(x => x.Region),

                VenueSortField.CountryCode =>
                    asc ? query.OrderBy(x => x.CountryCode) : query.OrderByDescending(x => x.CountryCode),

                VenueSortField.IsActive =>
                    asc ? query.OrderBy(x => x.IsActive) : query.OrderByDescending(x => x.IsActive),

                _ => asc ? query.OrderBy(x => x.Name) : query.OrderByDescending(x => x.Name)
            };

            return query;
        }
    }
}
