using EventHouse.Management.Api.Contracts.Seating.Seats;
using EventHouse.Management.Api.Contracts.Seating.Structure;
using System.Diagnostics.CodeAnalysis;

namespace EventHouse.Management.Api.Swagger.Examples.Data;

[ExcludeFromCodeCoverage]
internal static class SeatingMapStructureExampleData
{
    private static readonly Guid SectionId = ExampleConstants.SeatingSectionId;
    private static readonly Guid RowId = ExampleConstants.SeatingRowId;
    private static readonly Guid SeatId = ExampleConstants.SeatId;

    private static readonly string SectionName = "VIP";
    private static readonly int RowNumber = 1;
    private static readonly string RowLabel = "A";
    private static readonly int SeatNumber = 1;
    private static readonly string SeatLabel = "A1";

    internal static SeatingMapStructureResponse Result() => new()
    {
        Id = ExampleConstants.SeatingMapId,
        VenueId = ExampleConstants.VenueId,
        Name = "Main Floor Seating",
        Version = 1,
        IsActive = true,
        Sections =
        [
            new SeatingSectionStructureResponse
            {
                Id = SectionId,
                Name = SectionName,
                Rows =
                [
                    new SeatingRowStructureResponse
                    {
                        Id = RowId,
                        Number = RowNumber,
                        Label = RowLabel,
                        Seats =
                        [
                            new SeatResponse
                            {
                                Id = SeatId,
                                Number = SeatNumber,
                                Label = SeatLabel,
                            }
                        ],
                    }
                ],
            }
        ],
    };
}