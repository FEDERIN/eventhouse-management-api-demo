using EventHouse.Management.Application.DTOs;
using EventHouse.Management.Domain.Entities;

namespace EventHouse.Management.Application.Mappers.Artists;

internal class ArtistMapper
{
    public static ArtistDto ToDto(Artist artist)
    {
        return new ArtistDto
        {
            Id = artist.Id,
            Name = artist.Name,
            Category = ArtistCategoryMapper.ToApplication(artist.Category),
            Genres = [.. artist.Genres.Select(g => new ArtistGenreDto
            {
                GenreId = g.GenreId,
                Status = ArtistGenreStatusMapper.ToApplication(g.Status),
                IsPrimary = g.IsPrimary
            })]
        };
    }

    public static List<ArtistDto> ToDto(List<Artist> artists)
    {
        return [.. artists.Select(ToDto)];
    }
}
