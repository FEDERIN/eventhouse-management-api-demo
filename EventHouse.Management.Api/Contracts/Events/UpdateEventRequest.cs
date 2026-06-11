using EventHouse.Management.Api.Contracts.Common;
using Swashbuckle.AspNetCore.Annotations;
using System.ComponentModel.DataAnnotations;

namespace EventHouse.Management.Api.Contracts.Events
{
    public sealed record UpdateEventRequest
    {
        /// <summary>Event name.</summary>
        [Required]
        [MinLength(2)]
        [MaxLength(200)]
        [SwaggerSchema(
            Description = "Event name. Must be between 2 and 200 characters."
        )]
        public string Name { get; init; } = default!;

        /// <summary>Optional event description.</summary>
        [SwaggerSchema(
            Description = "Optional event description."
        )]
        public string? Description { get; init; }

        /// <summary>Geographical scope of the event.</summary>
        [Required]
        [SwaggerSchema(
            Description = "Defines who can see the event."
        )]
        [EnumDataType(typeof(EventScope))]
        public EventScope Scope { get; init; }
    }

}
