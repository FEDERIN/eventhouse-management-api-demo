namespace EventHouse.Management.Application.Exceptions;

public sealed class NotFoundException(string entity, object id) : Exception($"{entity} with id '{id}' was not found.")
{
    public string Code { get; } = $"{entity.ToUpperInvariant()}_NOT_FOUND";
    public string Title { get; } = $"{entity} not found";
}
