namespace EventHouse.Management.Domain.Exceptions.Seating.Sections;

public sealed class SeatingSectionCapacityBelowSeatCountException(
    Guid sectionId,
    int capacity,
    int seatCount)
    : DomainException(
        $"Section '{sectionId}' capacity '{capacity}' cannot be less than its current seat count of '{seatCount}'.");