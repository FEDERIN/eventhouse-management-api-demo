using EventHouse.Management.Api.Contracts.Common;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace EventHouse.Management.Api.Contracts.Venues;

public sealed record GetVenuesRequest : SortablePaginationRequest<VenueSortBy>
{
    /// <summary>Filter venues by name (contains match).</summary>
    [FromQuery(Name = "name"), MinLength(5), MaxLength(200)]
    public string? Name { get; init; }

    /// <summary>Physical address of the venue.</summary>
    [FromQuery(Name = "address"), MinLength(2), MaxLength(200)]
    public string? Address { get; init; }

    /// <summary>City where the venue is located.</summary>
    [FromQuery(Name = "city"), MinLength(2), MaxLength(100)]
    public string? City { get; init; }

    /// <summary>Region or state where the venue is located.</summary>
    [FromQuery(Name = "region"), MinLength(2), MaxLength(100)]
    public string? Region { get; init; }

    /// <summary>ISO-3166-1 alpha-2 country code.</summary>
    [RegularExpression("^[A-Z]{2}$")]
    [FromQuery(Name = "countryCode"), MinLength(2), MaxLength(2)]
    public string? CountryCode { get; init; }

    /// <summary>Maximum capacity of the venue. Null if not specified.</summary>
    [Range(0, int.MaxValue)]
    [FromQuery(Name = "capacity")]
    public int? Capacity { get; init; }

    /// <summary>Indicates whether the venue is currently active.</summary>
    [FromQuery(Name = "isActive")]
    public bool? IsActive { get; init; }
}
