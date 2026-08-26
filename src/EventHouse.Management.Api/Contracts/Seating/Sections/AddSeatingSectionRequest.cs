namespace EventHouse.Management.Api.Contracts.Seating.Sections;

public sealed record AddSeatingSectionRequest(
    string Name,
    bool IsNumbered,
    int Capacity);