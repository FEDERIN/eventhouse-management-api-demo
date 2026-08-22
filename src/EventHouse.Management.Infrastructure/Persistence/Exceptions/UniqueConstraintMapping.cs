namespace EventHouse.Management.Infrastructure.Persistence.Exceptions;

internal sealed record UniqueConstraintMapping(
    string? Code,
    string? Detail,
    bool ShouldIgnore = false);