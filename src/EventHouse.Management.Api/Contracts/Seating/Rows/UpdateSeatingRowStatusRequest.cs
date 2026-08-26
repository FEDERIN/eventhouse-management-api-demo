namespace EventHouse.Management.Api.Contracts.Seating.Rows;

public sealed record UpdateSeatingRowStatusRequest(
    bool IsActive);