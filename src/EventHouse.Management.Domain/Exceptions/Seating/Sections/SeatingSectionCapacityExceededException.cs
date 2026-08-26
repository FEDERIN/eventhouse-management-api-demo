namespace EventHouse.Management.Domain.Exceptions.Seating.Sections;

public sealed class SeatingSectionCapacityExceededException(
    Guid sectionId,
    int capacity)
    : DomainException(
        $"Section '{sectionId}' has reached its capacity of '{capacity}' seats.");