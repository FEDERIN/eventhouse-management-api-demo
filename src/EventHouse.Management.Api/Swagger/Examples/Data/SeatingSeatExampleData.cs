using EventHouse.Management.Api.Contracts.Seating.Seats;
using System.Diagnostics.CodeAnalysis;

namespace EventHouse.Management.Api.Swagger.Examples.Data;

[ExcludeFromCodeCoverage]
internal static class SeatingSeatExampleData
{
    private static readonly int Number = 1;
    private static readonly string Label = "Seat 1";

    internal static AddSeatingSeatRequest Add()
        => new(Number, Label);

    internal static UpdateSeatingSeatStatusRequest UpdateStatus()
    => new(IsActive: false);
}