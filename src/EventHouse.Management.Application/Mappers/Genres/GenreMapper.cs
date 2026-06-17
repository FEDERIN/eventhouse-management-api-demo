using EventHouse.Management.Application.DTOs;
using EventHouse.Management.Domain.Entities;


namespace EventHouse.Management.Application.Mappers.Genres;

internal sealed class GenreMapper
{
    public static GenreDto ToDto(Genre entity)
    {
        return new GenreDto
        {
            Id = entity.Id,
            Name = entity.Name
        };
    }

    public static IEnumerable<GenreDto> ToDto(IEnumerable<Genre> genres) => genres.Select(ToDto);
}
