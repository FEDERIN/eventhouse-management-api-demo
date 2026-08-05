using Npgsql;

namespace EventHouse.Management.Infrastructure.Resilience.Exceptions;

internal sealed class SqlTransientException(PostgresException innerException)
    : Exception(innerException.Message, innerException)
{
}