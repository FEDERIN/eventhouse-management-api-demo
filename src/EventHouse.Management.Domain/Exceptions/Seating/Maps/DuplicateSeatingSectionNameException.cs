namespace EventHouse.Management.Domain.Exceptions.Seating.Maps;

public sealed class DuplicateSeatingSectionNameException(
    Guid seatingMapId,
    string name)
    : DomainException(
        $"Section '{name}' already exists in seating map '{seatingMapId}'.");