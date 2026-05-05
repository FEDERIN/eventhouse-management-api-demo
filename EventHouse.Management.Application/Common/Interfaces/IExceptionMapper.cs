namespace EventHouse.Management.Application.Common.Interfaces;

public interface IExceptionMapper
{
    (int StatusCode, string ErrorCode, string Title, string Detail) Map(Exception exception);
}