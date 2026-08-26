using EventHouse.ShareKernel.Entities;
using System.Diagnostics.CodeAnalysis;

namespace EventHouse.Management.Domain.Entities;

public class Seat : Entity
{
    public Guid SeatingRowId { get; private set; }

    public int Number { get; private set; }

    public string Label { get; private set; } = string.Empty;

    public bool IsActive { get; private set; }

    [ExcludeFromCodeCoverage]
    public virtual SeatingRow? SeatingRow { get; private set; }

    private Seat()
    {
    }

    public Seat(
        Guid seatingRowId,
        int number,
        string label)
    {
        if (seatingRowId == Guid.Empty)
            throw new ArgumentException(
                "SeatingRowId cannot be empty.",
                nameof(seatingRowId));

        ValidateNumber(number);
        ValidateLabel(label);

        SeatingRowId = seatingRowId;
        Number = number;
        Label = label.Trim();
        IsActive = true;
    }

    public void Activate()
    {
        if (IsActive)
            return;

        IsActive = true;
    }

    public void Deactivate()
    {
        if (!IsActive)
            return;

        IsActive = false;
    }

    private static void ValidateNumber(int number)
    {
        if (number <= 0)
        {
            throw new ArgumentException(
                "Seat number must be greater than zero.",
                nameof(number));
        }
    }

    private static void ValidateLabel(string label)
    {
        if (string.IsNullOrWhiteSpace(label))
        {
            throw new ArgumentException(
                "Seat label is required.",
                nameof(label));
        }
    }
}