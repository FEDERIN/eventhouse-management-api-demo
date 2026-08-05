using Core.Idempotency.Exceptions;
using EventHouse.Management.Application.Common.Interfaces;
using EventHouse.Management.Application.Exceptions;
using EventHouse.Management.Domain.Exceptions;
using EventHouse.Management.Domain.Exceptions.Calendars;
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

            ConflictException ce =>
                (409, ce.Code, ce.Title, ce.Message, string.Empty),

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
            InvalidOperationException ioe =>
                (
                    500,
                    "INVALID_OPERATION",
                    "Invalid operation",
                    ioe.Message,
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