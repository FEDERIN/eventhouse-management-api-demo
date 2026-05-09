using EventHouse.Management.Application.Common.Interfaces;
using EventHouse.Management.Application.Exceptions;
using EventHouse.Management.Domain.Exceptions;
using EventHouse.Management.Domain.Exceptions.Artists;
using EventHouse.Management.Domain.Exceptions.Calendars;

namespace EventHouse.Management.Infrastructure.Errors;

internal sealed class ExceptionMapper : IExceptionMapper
{
    public (int StatusCode, string ErrorCode, string Title, string Detail) Map(Exception ex)
    {
        return ex switch
        {
            DuplicateHeadlinerException dhe => (409, "DUPLICATE_HEADLINER", "Headliner already assigned", dhe.Message),
            ArtistIsNotHeadlinerException ahe => (409, "ARTIST_NOT_HEADLINER", "Invalid Headliner swap", ahe.Message),
            StageOverlapException soe => (409, "STAGE_OVERLAP", "Stage schedule conflict", soe.Message),
            PerformanceDatesRequiredException pdre => (409, "REQUIRED_DATES", "Missing schedule", pdre.Message),
            CannotRemovePublishedHeadlinerException crphe => (409, "CANNOT_REMOVE_PUBLISHED_HEADLINER", "Cannot remove published headliner", crphe.Message),
            NotFoundException nf => (404, nf.Code, nf.Title, nf.Message),
            ConflictException ce => (409, ce.Code, ce.Title, ce.Message),
            NotAssociatedException nae => (404, "RESOURCE_NOT_ASSOCIATED", "Resource not associated", nae.Message),
            ArgumentException ae => (400, "BAD_REQUEST", "Bad request", ae.Message),
            InvalidOperationException ioe => (409, "CONFLICT", "Conflict", ioe.Message),
            _ => ( 500, "UNEXPECTED_ERROR", "Unexpected error", "An unexpected error occurred.")
        };
    }
}