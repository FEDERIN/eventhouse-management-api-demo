namespace EventHouse.Management.Domain.Exceptions.Seating.Maps;

public sealed class InactiveSeatingMapException(
    Guid seatingMapId)
    : DomainException(
        $"Seating map '{seatingMapId}' is inactive.");