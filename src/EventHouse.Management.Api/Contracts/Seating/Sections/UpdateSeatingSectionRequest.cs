namespace EventHouse.Management.Api.Contracts.Seating.Sections;

public sealed record UpdateSeatingSectionRequest(
    string Name,
    bool IsNumbered,
    int Capacity);