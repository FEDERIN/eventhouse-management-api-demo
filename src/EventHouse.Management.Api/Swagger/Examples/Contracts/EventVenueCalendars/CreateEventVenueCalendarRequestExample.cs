using EventHouse.Management.Api.Contracts.EventVenueCalendars;
using EventHouse.Management.Api.Swagger.Examples.Data;
using Swashbuckle.AspNetCore.Filters;
using System.Diagnostics.CodeAnalysis;

namespace EventHouse.Management.Api.Swagger.Examples.Contracts.EventVenueCalendars;

[ExcludeFromCodeCoverage]
internal sealed class CreateEventVenueCalendarRequestExample : IExamplesProvider<CreateEventVenueCalendarRequest>
{
    public CreateEventVenueCalendarRequest GetExamples() => EventVenueCalendarExampleData.Create();
}
