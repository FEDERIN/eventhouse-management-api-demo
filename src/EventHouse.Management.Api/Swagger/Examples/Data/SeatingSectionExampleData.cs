using EventHouse.Management.Api.Contracts.Seating.Sections;
using System.Diagnostics.CodeAnalysis;

namespace EventHouse.Management.Api.Swagger.Examples.Data;

[ExcludeFromCodeCoverage]
internal static class SeatingSectionExampleData
{
    private static readonly string Name = "VIP";

    internal static AddSeatingSectionRequest Add() => new(Name, true, 100);

    internal static UpdateSeatingSectionRequest Update() => new(Name, true, 200);

    internal static UpdateSeatingSectionStatusRequest UpdateStatus()
    => new(IsActive: false);
}