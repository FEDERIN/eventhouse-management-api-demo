using Npgsql;

namespace EventHouse.Management.Infrastructure.Resilience.Exceptions;

internal static class PostgresExceptionMapper
{
    public static Exception Map(Exception exception)
    {
        if (exception is not PostgresException postgres)
            return exception;

        return postgres.SqlState switch
        {
            // Serialization failure
            "40001" => new SqlTransientException(postgres),

            // Deadlock detected
            "40P01" => new SqlTransientException(postgres),

            // Too many connections
            "53300" => new SqlTransientException(postgres),

            // Cannot connect now
            "57P03" => new SqlTransientException(postgres),

            // Connection Exception
            "08000" => new SqlTransientException(postgres),

            // SQL Client Unable To Establish Connection
            "08001" => new SqlTransientException(postgres),

            // Connection Does Not Exist
            "08003" => new SqlTransientException(postgres),

            // Connection Failure
            "08006" => new SqlTransientException(postgres),

            // Transaction Resolution Unknown
            "08007" => new SqlTransientException(postgres),

            _ => exception
        };
    }
}