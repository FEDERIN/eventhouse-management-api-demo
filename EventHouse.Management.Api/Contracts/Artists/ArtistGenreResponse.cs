namespace EventHouse.Management.Api.Contracts.Artists;

public sealed record ArtistGenreResponse
{
    public Guid GenreId { get; set; }
    public bool IsPrimary { get; set; }
    public ArtistGenreStatus Status { get; set; }
}
