using EventHouse.Management.Api.Contracts.Venues;
using EventHouse.Management.Application.Queries.Venues.GetAll;

namespace EventHouse.Management.Api.Mappers.Venues;

public static class GetAllVenuesQueryMapper
{
        public static GetAllVenuesQuery FromContract(GetVenuesRequest request)
            => new()
            {
                Name = request.Name,
                Address = request.Address,
                City = request.City,
                Region = request.Region,
                CountryCode = request.CountryCode,
                Capacity = request.Capacity,
                IsActive = request.IsActive,
                Page = request.Page,
                PageSize = request.PageSize,
                SortBy = VenueSortMapper.ToApplication(request.SortBy),
                SortDirection = SortDirectionMapper.ToApplication(request.SortDirection)
            };
}
