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

    public static IReadOnlyList<GenreDto> ToDto(IReadOnlyList<Genre> genres)
    {
        return [.. genres.Select(ToDto)];
    }
}
