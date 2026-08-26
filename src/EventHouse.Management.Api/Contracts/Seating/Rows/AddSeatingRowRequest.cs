namespace EventHouse.Management.Api.Contracts.Seating.Rows;

public sealed record AddSeatingRowRequest(
    int Number,
    string Label);