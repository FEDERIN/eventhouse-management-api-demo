using EventHouse.Management.Api.Contracts.SeatingMaps;
using EventHouse.Management.Application.Queries.SeatingMaps.GetAll;

namespace EventHouse.Management.Api.Mappers.SeatingMaps;

internal sealed class GetAllSeatingMapsQueryMapper
{
    public static GetAllSeatingMapsQuery FromContract(GetSeatingMapsRequest request)
    {
        return new GetAllSeatingMapsQuery
        {
            Name = request.Name,
            VenueId = request.VenueId,
            IsActive = request.IsActive,
        };
    }
}
