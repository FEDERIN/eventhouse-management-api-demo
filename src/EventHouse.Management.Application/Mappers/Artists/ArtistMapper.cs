using EventHouse.Management.Application.Commands.Artists.Create;
using EventHouse.Management.Application.DTOs;
using EventHouse.Management.Domain.Entities;

namespace EventHouse.Management.Application.Mappers.Artists;

internal class ArtistMapper
{
    public static Artist ToEntity(CreateArtistCommand request)
    {
        return new Artist(
            Guid.NewGuid(),
            request.Name.Trim(),
            ArtistCategoryMapper.ToDomainRequired(request.Category));
    }

    public static ArtistDto ToDtoWithRelation(Artist artist)
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

    public static ArtistDto ToDto(Artist artist)
    {
        return new ArtistDto
        {
            Id = artist.Id,
            Name = artist.Name,
            Category = ArtistCategoryMapper.ToApplication(artist.Category)
        };
    }

    public static IEnumerable<ArtistDto> ToDto(IEnumerable<Artist> artists) => artists.Select(ToDto);
}
