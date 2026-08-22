using EventHouse.Management.Application.Common.Interfaces;
using EventHouse.Management.Application.DTOs;
using EventHouse.Management.Application.Exceptions;
using EventHouse.Management.Application.Mappers.ArtistPerformances;
using MediatR;

namespace EventHouse.Management.Application.Queries.ArtistPerformances.GetById;

internal class GetArtistPerformanceByIdQueryHandler(IArtistPerformanceRepository repository) 
    : IRequestHandler<GetArtistPerformanceByIdQuery, ArtistPerformanceDto>
{
    public async Task<ArtistPerformanceDto> Handle(GetArtistPerformanceByIdQuery request, CancellationToken ct)
    {
        var entity = await repository.GetByIdAsync(request.Id, ct)
            ?? throw new NotFoundException("ArtistPerformance", request.Id);

        return ArtistPerformanceMapper.ToDto(entity);
    }
}