namespace EventHouse.Management.Domain.Exceptions.Seating.Rows;

public sealed class InactiveSeatingRowException(
    Guid rowId)
    : DomainException(
        $"Row '{rowId}' is inactive.");