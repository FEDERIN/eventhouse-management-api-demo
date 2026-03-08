using EventHouse.Management.Domain.Enums;
using EventHouse.ShareKernel.Entities;

namespace EventHouse.Management.Domain.Entities;

public class EventVenue : Entity
{
    public Guid EventId { get; private set; }
    public Guid VenueId { get; private set; }
    public EventVenueStatus Status { get; private set; }
    public virtual Event? Event { get; private set; }
    public virtual Venue? Venue { get; private set; }

    private EventVenue() { }

    public EventVenue(Guid id, Guid eventId, Guid venueId, EventVenueStatus status)
    {
        if (id == Guid.Empty)
            throw new ArgumentException("Id cannot be empty.", nameof(id));
        if (eventId == Guid.Empty)
            throw new ArgumentException("EventId cannot be empty.", nameof(eventId));
        if (venueId == Guid.Empty)
            throw new ArgumentException("VenueId cannot be empty.", nameof(venueId));

        Id = id;
        EventId = eventId;
        VenueId = venueId;
        Status = status;
    }

    public bool ChangeStatus(EventVenueStatus newStatus)
    {
        if (Status == newStatus)
            return false;

        Status = newStatus;

        return true;
    }
}