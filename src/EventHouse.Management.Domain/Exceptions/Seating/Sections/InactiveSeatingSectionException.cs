namespace EventHouse.Management.Domain.Exceptions.Seating.Sections;

public sealed class InactiveSeatingSectionException(
    Guid sectionId)
    : DomainException(
        $"Section '{sectionId}' is inactive.");