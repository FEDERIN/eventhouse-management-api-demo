using EventHouse.Management.Domain.Exceptions.Seating.Rows;
using EventHouse.ShareKernel.Entities;
using System.Diagnostics.CodeAnalysis;

namespace EventHouse.Management.Domain.Entities;

public class SeatingRow : Entity
{
    public Guid SeatingSectionId { get; private set; }

    public int Number { get; private set; }

    public string Label { get; private set; } = string.Empty;

    public bool IsActive { get; private set; }

    [ExcludeFromCodeCoverage]
    public virtual SeatingSection? SeatingSection { get; private set; }

    private readonly List<Seat> _seats = [];

    public virtual IReadOnlyCollection<Seat> Seats =>
        _seats.AsReadOnly();

    private SeatingRow()
    {
    }

    public SeatingRow(
        Guid seatingSectionId,
        int number,
        string label)
    {
        if (seatingSectionId == Guid.Empty)
            throw new ArgumentException(
                "SeatingSectionId cannot be empty.",
                nameof(seatingSectionId));

        ValidateNumber(number);
        ValidateLabel(label);

        SeatingSectionId = seatingSectionId;
        Number = number;
        Label = label.Trim();
        IsActive = true;
    }

    public void Activate()
    {
        if (IsActive)
            return;

        IsActive = true;

        foreach (var seat in _seats)
        {
            seat.Activate();
        }
    }

    public void Deactivate()
    {
        if (!IsActive)
            return;

        IsActive = false;

        foreach (var seat in _seats)
        {
            seat.Deactivate();
        }
    }

    public void AddSeat(
        int number,
        string label)
    {
        if (!IsActive)
        {
            throw new InactiveSeatingRowCannotContainSeatsException(Id);
        }

        if (_seats.Any(x => x.Number == number))
        {
            throw new DuplicateSeatingSeatNumberException(
                Id,
                number);
        }

        _seats.Add(
            new Seat(
                Id,
                number,
                label));
    }

    public void ActivateSeat(Guid seatId)
    {
        EnsureActive();

        var seat = _seats.FirstOrDefault(
            x => x.Id == seatId)
            ?? throw new InvalidOperationException(
                $"Seat '{seatId}' does not exist in row '{Id}'.");

        seat.Activate();
    }

    public void DeactivateSeat(Guid seatId)
    {
        var seat = _seats.FirstOrDefault(
            x => x.Id == seatId)
            ?? throw new InvalidOperationException(
                $"Seat '{seatId}' does not exist in row '{Id}'.");

        seat.Deactivate();
    }

    private void EnsureActive()
    {
        if (IsActive)
            return;

        throw new InactiveSeatingRowException(Id);
    }

    private static void ValidateNumber(int number)
    {
        if (number <= 0)
        {
            throw new ArgumentException(
                "Row number must be greater than zero.",
                nameof(number));
        }
    }

    private static void ValidateLabel(string label)
    {
        if (string.IsNullOrWhiteSpace(label))
        {
            throw new ArgumentException(
                "Row label is required.",
                nameof(label));
        }
    }
}