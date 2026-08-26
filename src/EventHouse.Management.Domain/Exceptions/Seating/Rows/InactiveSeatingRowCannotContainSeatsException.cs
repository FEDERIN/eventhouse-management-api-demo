namespace EventHouse.Management.Domain.Exceptions.Seating.Rows;

public sealed class InactiveSeatingRowCannotContainSeatsException(
    Guid rowId)
    : DomainException(
        $"Inactive row '{rowId}' cannot contain active seats.");