using Swashbuckle.AspNetCore.Annotations;

namespace EventHouse.Management.Api.Contracts.Genres;

public class Genre
{
    /// <summary>Unique identifier of the genre.</summary>
    public Guid Id { get; set; }

    /// <summary>Public display name of the genre.</summary>
    [SwaggerSchema(
        Description = "Public display name of the genre.")]
    public string Name { get; set; } = null!;
}
