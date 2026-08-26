namespace EventHouse.Management.Domain.Exceptions.Seating.Sections;

public sealed class NonNumberedSectionException(
    Guid sectionId)
    : DomainException(
        $"Section '{sectionId}' is non-numbered and cannot contain rows or seats.");