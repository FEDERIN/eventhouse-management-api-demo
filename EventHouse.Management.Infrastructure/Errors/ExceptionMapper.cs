using EventHouse.Management.Application.Common.Interfaces;
using EventHouse.Management.Application.Exceptions;
using EventHouse.Management.Domain.Exceptions;

namespace EventHouse.Management.Infrastructure.Errors;

internal sealed class ExceptionMapper : IExceptionMapper
{
    public (int StatusCode, string ErrorCode, string Title, string Detail) Map(Exception ex)
    {
        return ex switch
        {
            NotFoundException nf => (404, nf.Code, nf.Title, nf.Message),
            ConflictException ce => (409, ce.Code, ce.Title, ce.Message),
            NotAssociatedException nae => (404, "RESOURCE_NOT_ASSOCIATED", "Resource not associated", nae.Message),
            ArgumentException ae => (400, "BAD_REQUEST", "Bad request", ae.Message),
            InvalidOperationException ioe => (409, "CONFLICT", "Conflict", ioe.Message),
            _ => ( 500, "UNEXPECTED_ERROR", "Unexpected error", "An unexpected error occurred.")
        };
    }
}