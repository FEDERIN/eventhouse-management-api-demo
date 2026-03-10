using EventHouse.Management.Api.Contracts.EventVenueCalendars;
using EventHouse.Management.Api.Swagger.Examples.Data;
using Swashbuckle.AspNetCore.Filters;
using System.Diagnostics.CodeAnalysis;

namespace EventHouse.Management.Api.Swagger.Examples.Requests.EventVenueCalendars;

[ExcludeFromCodeCoverage]
internal sealed class GetEventVenueCalendarsRequestExample : IExamplesProvider<GetEventVenueCalendarsRequest>
{
    public GetEventVenueCalendarsRequest GetExamples() => EventVenueCalendarExampleData.Get();
}
