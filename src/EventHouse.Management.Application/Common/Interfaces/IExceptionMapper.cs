namespace EventHouse.Management.Application.Common.Interfaces;

public interface IExceptionMapper
{
    (int StatusCode, string ErrorCode, string Title, string Detail, string Type) Map(Exception exception);
}