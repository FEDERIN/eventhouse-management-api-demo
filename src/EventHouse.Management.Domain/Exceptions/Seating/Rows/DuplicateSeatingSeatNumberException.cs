namespace EventHouse.Management.Domain.Exceptions.Seating.Rows;

public sealed class DuplicateSeatingSeatNumberException(
    Guid rowId,
    int number)
    : DomainException(
        $"Seat number '{number}' already exists in row '{rowId}'.");