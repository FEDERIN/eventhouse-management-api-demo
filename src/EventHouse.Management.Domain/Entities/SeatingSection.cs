using EventHouse.Management.Domain.Exceptions.Seating.Sections;
using EventHouse.ShareKernel.Entities;
using System.Diagnostics.CodeAnalysis;

namespace EventHouse.Management.Domain.Entities;

public class SeatingSection : Entity
{
    public Guid SeatingMapId { get; private set; }

    public string Name { get; private set; } = string.Empty;

    public bool IsNumbered { get; private set; }

    public int Capacity { get; private set; }

    public bool IsActive { get; private set; }

    [ExcludeFromCodeCoverage]
    public virtual SeatingMap? SeatingMap { get; private set; }

    private readonly List<SeatingRow> _rows = [];

    public virtual IReadOnlyCollection<SeatingRow> Rows =>
        _rows.AsReadOnly();

    private SeatingSection()
    {
    }

    public SeatingSection(
        Guid seatingMapId,
        string name,
        bool isNumbered,
        int capacity)
    {
        if (seatingMapId == Guid.Empty)
            throw new ArgumentException(
                "SeatingMapId cannot be empty.",
                nameof(seatingMapId));

        ValidateName(name);
        ValidateCapacity(capacity);

        SeatingMapId = seatingMapId;
        Name = name.Trim();
        IsNumbered = isNumbered;
        Capacity = capacity;
        IsActive = true;
    }

    public void Update(
        string name,
        int capacity)
    {
        ValidateName(name);
        ValidateCapacity(capacity);
        ValidateCapacityAgainstSeats(capacity);

        Name = name.Trim();
        Capacity = capacity;
    }

    public void Activate()
    {
        if (IsActive)
            return;

        IsActive = true;

        foreach (var row in _rows)
        {
            row.Activate();
        }
    }

    public void Deactivate()
    {
        if (!IsActive)
            return;

        IsActive = false;

        foreach (var row in _rows)
        {
            row.Deactivate();
        }
    }

    public void AddRow(
        int number,
        string label)
    {
        EnsureActive();
        EnsureNumbered();

        if (_rows.Any(x => x.Number == number))
        {
            throw new RowNumberAlreadyExistsException(number, Id);
        }

        _rows.Add(
            new SeatingRow(
                Id,
                number,
                label));
    }

    public void ActivateRow(Guid rowId)
    {
        EnsureActive();
        EnsureNumbered();

        var row = GetRow(rowId);

        row.Activate();
    }

    public void DeactivateRow(Guid rowId)
    {
        var row = GetRow(rowId);

        row.Deactivate();
    }

    public void AddSeat(
        Guid rowId,
        int number,
        string label)
    {
        EnsureActive();
        EnsureNumbered();
        EnsureCapacityAvailable();

        var row = GetRow(rowId);

        row.AddSeat(
            number,
            label);
    }

    public void ActivateSeat(
        Guid rowId,
        Guid seatId)
    {
        EnsureActive();
        EnsureNumbered();

        var row = GetRow(rowId);

        row.ActivateSeat(seatId);
    }

    public void DeactivateSeat(
        Guid rowId,
        Guid seatId)
    {
        var row = GetRow(rowId);

        row.DeactivateSeat(seatId);
    }

    private SeatingRow GetRow(Guid rowId)
    {
        return _rows.FirstOrDefault(
            x => x.Id == rowId)
            ?? throw new InvalidOperationException(
                $"Seating row '{rowId}' does not exist in section '{Id}'.");
    }

    private void EnsureActive()
    {
        if (IsActive)
            return;
        throw new InactiveSeatingSectionException(Id);
    }

    private void EnsureNumbered()
    {
        if (IsNumbered)
            return;

        throw new NonNumberedSectionException(Id);
    }

    private void EnsureCapacityAvailable()
    {
        var seatCount = GetSeatCount();

        if (seatCount >= Capacity)
        {
            throw new SeatingSectionCapacityExceededException(
                Id,
                Capacity);
        }
    }

    private void ValidateCapacityAgainstSeats(int capacity)
    {
        var seatCount = GetSeatCount();

        if (capacity < seatCount)
        {
            throw new SeatingSectionCapacityBelowSeatCountException(
                Id,
                capacity,
                seatCount);
        }
    }

    private int GetSeatCount()
    {
        return _rows.Sum(x => x.Seats.Count);
    }

    private static void ValidateName(string name)
    {
        if (string.IsNullOrWhiteSpace(name))
        {
            throw new ArgumentException(
                "Section name is required.",
                nameof(name));
        }
    }

    private static void ValidateCapacity(int capacity)
    {
        if (capacity <= 0)
        {
            throw new ArgumentException(
                "Section capacity must be greater than zero.",
                nameof(capacity));
        }
    }
}