using EventHouse.Management.Domain.Exceptions;
using EventHouse.Management.Domain.Exceptions.Seating.Maps;
using EventHouse.ShareKernel.Entities;

namespace EventHouse.Management.Domain.Entities;

public class SeatingMap : Entity
{
    public Guid VenueId { get; private set; }

    public string Name { get; private set; } = default!;

    public int Version { get; private set; }

    public bool IsActive { get; private set; }

    public DateTime CreatedAtUtc { get; private set; }

    private readonly List<SeatingSection> _sections = [];

    public virtual IReadOnlyCollection<SeatingSection> Sections =>
        _sections.AsReadOnly();

    private SeatingMap()
    {
    }

    public SeatingMap(
        Guid id,
        Guid venueId,
        string name,
        int version)
    {
        if (id == Guid.Empty)
            throw new ArgumentException(
                "Id cannot be empty.",
                nameof(id));

        if (venueId == Guid.Empty)
            throw new ArgumentException(
                "VenueId cannot be empty.",
                nameof(venueId));

        Id = id;
        VenueId = venueId;
        Name = name;
        Version = version;
        IsActive = true;
        CreatedAtUtc = DateTime.UtcNow;
    }

    public void Update(
        string name,
        int version)
    {
        Name = name;
        Version = version;
    }

    public void Activate()
    {
        if (IsActive)
            return;

        IsActive = true;

        foreach (var section in _sections)
        {
            section.Activate();
        }
    }

    public void Deactivate()
    {
        if (!IsActive)
            return;

        IsActive = false;

        foreach (var section in _sections)
        {
            section.Deactivate();
        }
    }

    public void AddSection(
        string name,
        bool isNumbered,
        int capacity)
    {
        EnsureActive();

        if (_sections.Any(x =>
                string.Equals(
                    x.Name,
                    name,
                    StringComparison.OrdinalIgnoreCase)))
        {
            throw new DuplicateSeatingSectionNameException(
                Id, name);
        }

        _sections.Add(
            new SeatingSection(
                Id,
                name,
                isNumbered,
                capacity));
    }

    public void UpdateSection(
        Guid sectionId,
        string name,
        int capacity)
    {
        EnsureActive();

        var section = GetSection(sectionId);

        if (_sections.Any(x =>
                x.Id != sectionId &&
                string.Equals(
                    x.Name,
                    name,
                    StringComparison.OrdinalIgnoreCase)))
        {
            throw new DuplicateSeatingSectionNameException(
                Id, name);
        }

        section.Update(
            name,
            capacity);
    }

    public void ActivateSection(Guid sectionId)
    {
        EnsureActive();

        var section = GetSection(sectionId);

        section.Activate();
    }

    public void DeactivateSection(Guid sectionId)
    {
        var section = GetSection(sectionId);

        section.Deactivate();
    }

    public void AddRow(
        Guid sectionId,
        int number,
        string label)
    {
        EnsureActive();

        var section = GetSection(sectionId);

        section.AddRow(
            number,
            label);
    }

    public void ActivateRow(
        Guid sectionId,
        Guid rowId)
    {
        EnsureActive();

        var section = GetSection(sectionId);

        section.ActivateRow(rowId);
    }

    public void DeactivateRow(
        Guid sectionId,
        Guid rowId)
    {
        var section = GetSection(sectionId);

        section.DeactivateRow(rowId);
    }

    public void AddSeat(
        Guid sectionId,
        Guid rowId,
        int number,
        string label)
    {
        EnsureActive();

        var section = GetSection(sectionId);

        section.AddSeat(
            rowId,
            number,
            label);
    }

    public void ActivateSeat(
        Guid sectionId,
        Guid rowId,
        Guid seatId)
    {
        EnsureActive();

        var section = GetSection(sectionId);

        section.ActivateSeat(
            rowId,
            seatId);
    }

    public void DeactivateSeat(
        Guid sectionId,
        Guid rowId,
        Guid seatId)
    {
        var section = GetSection(sectionId);

        section.DeactivateSeat(
            rowId,
            seatId);
    }

    private SeatingSection GetSection(Guid sectionId)
    {
        return _sections.FirstOrDefault(
            x => x.Id == sectionId)
            ?? throw new NotAssociatedException(
                "SeatingMap",
                "SeatingSection",
                Id,
                sectionId);
    }

    private void EnsureActive()
    {
        if (IsActive)
            return;

        throw new InactiveSeatingMapException(Id);
    }
}