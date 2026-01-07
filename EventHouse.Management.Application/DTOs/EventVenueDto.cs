
using EventHouse.Management.Application.Common.Enums;

namespace EventHouse.Management.Application.DTOs;

public class EventVenueDto
{
    public Guid Id { get; set; }
    public Guid EventId { get; set; }
    public Guid VenueId { get; set; }
    public EventVenueStatus Status { get; set; }
}
