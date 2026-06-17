using Swashbuckle.AspNetCore.Annotations;
using System.ComponentModel.DataAnnotations;

namespace EventHouse.Management.Api.Contracts.Genres;

public sealed record CreateGenreRequest
{
    /// <summary>
    /// Name of the genre. Must be between 2 and 200 characters.
    ///  </summary>
    //<example>Rock</example>
    [Required]
    [MinLength(2)]
    [MaxLength(200)]
    [SwaggerSchema(
        Description = "Genre name. Must be between 2 and 200 characters."
    )]
    public string Name { get; init; } = string.Empty;
}
