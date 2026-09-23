using Core.Http.ProblemDetails;
using EventHouse.Management.Application.Common.Interfaces;

namespace EventHouse.Management.Api.ErrorHandling;

/// <summary>
/// Adapts EventHouse's application exception vocabulary to CoreSystem HTTP problem details.
/// </summary>
public sealed class EventHouseExceptionProblemMapper(IExceptionMapper exceptionMapper)
    : IExceptionProblemMapper
{
    public ProblemDescriptor Map(Exception exception)
    {
        var (statusCode, errorCode, title, detail, type) = exceptionMapper.Map(exception);

        return new ProblemDescriptor(
            statusCode,
            errorCode,
            title,
            detail,
            string.IsNullOrWhiteSpace(type) ? $"urn:eventhouse:error:{errorCode}" : type);
    }
}
