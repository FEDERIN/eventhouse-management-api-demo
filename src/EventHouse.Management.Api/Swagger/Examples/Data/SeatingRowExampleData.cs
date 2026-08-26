using EventHouse.Management.Api.Contracts.Seating.Rows;
using System.Diagnostics.CodeAnalysis;

namespace EventHouse.Management.Api.Swagger.Examples.Data;

[ExcludeFromCodeCoverage]
internal static class SeatingRowExampleData
{
    private static readonly int Number = 1;
    private static readonly string Label = "A";

    internal static AddSeatingRowRequest Add() =>
        new(Number, Label);

    internal static UpdateSeatingRowStatusRequest UpdateStatus()
    => new(IsActive: false);
}