using Core.Idempotency.Exceptions;
using EventHouse.Management.Application.Common.Interfaces;
using EventHouse.Management.Application.Exceptions;
using EventHouse.Management.Domain.Exceptions;
using EventHouse.Management.Domain.Exceptions.Calendars;
using EventHouse.Management.Domain.Exceptions.Seating.Maps;
using EventHouse.Management.Domain.Exceptions.Seating.Rows;
using EventHouse.Management.Domain.Exceptions.Seating.Sections;
using EventHouse.Management.Domain.Exceptions.Venues;
using Npgsql;

namespace EventHouse.Management.Infrastructure.Errors;

internal sealed class ExceptionMapper : IExceptionMapper
{
    public (int StatusCode, string ErrorCode, string Title, string Detail, string Type) Map(Exception ex)
    {
        return ex switch
        {
            // Business
            DuplicateHeadlinerException dhe =>
                (409, "DUPLICATE_HEADLINER", "Headliner already assigned", dhe.Message, string.Empty),

            ArtistIsNotHeadlinerException ahe =>
                (409, "ARTIST_NOT_HEADLINER", "Invalid headliner swap", ahe.Message, string.Empty),

            StageOverlapException soe =>
                (409, "STAGE_OVERLAP", "Stage schedule conflict", soe.Message, string.Empty),

            PerformanceDatesRequiredException pdre =>
                (409, "REQUIRED_DATES", "Missing schedule", pdre.Message, string.Empty),

            CannotRemovePublishedHeadlinerException crphe =>
                (409, "CANNOT_REMOVE_PUBLISHED_HEADLINER", "Cannot remove published headliner", crphe.Message, string.Empty),

            InvalidTimeRangeException itre =>
                (409, "INVALID_TIME_RANGE", "Invalid time range", itre.Message, string.Empty),

            InvalidLatitudeException ile =>
                (409, "INVALID_LATITUDE", "Invalid latitude", ile.Message, string.Empty),

            InvalidLongitudeException ole =>
                (409, "INVALID_LONGITUDE", "Invalid longitude", ole.Message, string.Empty),

            PerformanceOutsideCalendarException poce =>
                (
                    409,
                    "PERFORMANCE_OUTSIDE_CALENDAR",
                    "Performance outside calendar",
                    poce.Message,
                    string.Empty
                ),

            InvalidCalendarStatusTransitionException icste =>
                (
                    409,
                    "INVALID_CALENDAR_STATUS_TRANSITION",
                    "Invalid calendar status transition",
                    icste.Message,
                    string.Empty
                ),

            RowNumberAlreadyExistsException rnae =>
                (
                    409,
                    "ROW_NUMBER_ALREADY_EXISTS",
                    "Row number already exists",
                    rnae.Message,
                    string.Empty
                ),

            NonNumberedSectionException nnse =>
                (
                    409,
                    "NON_NUMBERED_SECTION",
                    "Section does not support rows or seats",
                    nnse.Message,
                    string.Empty
                ),

            InactiveSeatingSectionException ise =>
                (
                    409,
                    "INACTIVE_SEATING_SECTION",
                    "Seating section is inactive",
                    ise.Message,
                    string.Empty
                ),

            SeatingSectionCapacityExceededException sce =>
                (
                    409,
                    "SEATING_SECTION_CAPACITY_EXCEEDED",
                    "Seating section capacity exceeded",
                    sce.Message,
                    string.Empty
                ),

            SeatingSectionCapacityBelowSeatCountException cbsce =>
                (
                    409,
                    "SEATING_SECTION_CAPACITY_BELOW_SEAT_COUNT",
                    "Seating section capacity is below the current seat count",
                    cbsce.Message,
                    string.Empty
                ),

            ConflictException ce =>
                (409, ce.Code, ce.Title, ce.Message, string.Empty),

            InactiveSeatingRowException isre =>
                (
                    409,
                    "INACTIVE_SEATING_ROW",
                    "Seating row is inactive",
                    isre.Message,
                    string.Empty
                ),

            InactiveSeatingRowCannotContainSeatsException ircse =>
                (
                    409,
                    "INACTIVE_SEATING_ROW_CANNOT_CONTAIN_SEATS",
                    "Inactive seating row cannot contain seats",
                    ircse.Message,
                    string.Empty
                ),

            DuplicateSeatingSeatNumberException dssne =>
                (
                    409,
                    "SEAT_NUMBER_ALREADY_EXISTS",
                    "Seat number already exists",
                    dssne.Message,
                    string.Empty
                ),

            DuplicateSeatingSectionNameException dsne =>
                (
                    409,
                    "SEATING_SECTION_NAME_ALREADY_EXISTS",
                    "Seating section name already exists",
                    dsne.Message,
                    string.Empty
                ),

            InactiveSeatingMapException isme =>
                (
                    409,
                    "INACTIVE_SEATING_MAP",
                    "Seating map is inactive",
                    isme.Message,
                    string.Empty
                ),

            // Idempotency
            IdempotencyFingerprintMismatchException ifm =>
                (
                    409,
                    IdempotencyFingerprintMismatchException.Code,
                    IdempotencyFingerprintMismatchException.Title,
                    ifm.Message,
                    IdempotencyFingerprintMismatchException.Type
                ),

            // Not Found
            NotFoundException nf =>
                (404, nf.Code, nf.Title, nf.Message, string.Empty),

            NotAssociatedException nae =>
                (404, "RESOURCE_NOT_ASSOCIATED", "Resource not associated", nae.Message, string.Empty),

            // Validation
            ArgumentException ae =>
                (400, "BAD_REQUEST", "Bad request", ae.Message, string.Empty),

            // Infrastructure
            InvalidOperationException ioe
                when ioe.InnerException is NpgsqlException =>
                (
                    503,
                    "DATABASE_UNAVAILABLE",
                    "Database unavailable",
                    "The database is temporarily unavailable.",
                    string.Empty
                ),

            // Unexpected
            InvalidOperationException =>
                (
                    500,
                    "INTERNAL_SERVER_ERROR",
                    "Invalid operation",
                    "An unexpected error occurred.",
                    string.Empty
                ),
            _ =>
                (
                    500,
                    "UNEXPECTED_ERROR",
                    "Unexpected error",
                    "An unexpected error occurred.",
                    string.Empty
                )
        };
    }
}