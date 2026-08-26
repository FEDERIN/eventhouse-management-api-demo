namespace EventHouse.Management.Api.Contracts.Seating.Seats;

public sealed record AddSeatingSeatRequest(
    int Number,
    string Label);