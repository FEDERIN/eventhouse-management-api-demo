namespace EventHouse.Management.Application.Exceptions;
public sealed class ConflictException : Exception
{
    public string Code { get; }

    public ConflictException(string code, string message)
        : base(message)
    {
        Code = code;
    }
}
