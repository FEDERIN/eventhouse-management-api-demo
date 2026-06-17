namespace EventHouse.Management.Application.Exceptions;

public sealed class ConflictException(string code, string title, string detail) : Exception(detail)
{
    public string Code { get; } = code;
    public string Title { get; } = title;
}
