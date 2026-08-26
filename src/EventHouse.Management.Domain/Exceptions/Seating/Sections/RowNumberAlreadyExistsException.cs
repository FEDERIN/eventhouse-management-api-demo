namespace EventHouse.Management.Domain.Exceptions.Seating.Sections;

public sealed class RowNumberAlreadyExistsException(
    int number,
    Guid sectionId)
    : Exception(
        $"Row number '{number}' already exists in section '{sectionId}'.");
