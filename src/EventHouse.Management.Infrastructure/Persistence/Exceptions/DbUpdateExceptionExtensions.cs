using Microsoft.EntityFrameworkCore;
using Npgsql;

namespace EventHouse.Management.Infrastructure.Persistence.Exceptions;

public static class DbUpdateExceptionExtensions
{
    // PostgreSQL Error Code for unique_violation: 23505
    private const string PostgresUniqueViolationCode = "23505";

    public static bool IsUniqueViolation(this DbUpdateException ex)
    {
        // Check if the inner exception is a PostgresException and matches the 23505 code
        return ex.InnerException is PostgresException { SqlState: PostgresUniqueViolationCode };
    }

    public static bool IsUniqueViolation(this DbUpdateException ex, string constraintName)
    {
        if (ex.InnerException is PostgresException pgEx && pgEx.SqlState == PostgresUniqueViolationCode)
        {
            // PostgreSQL provides the specific constraint name in the 'ConstraintName' property
            // This is much safer than parsing a string message
            return string.Equals(pgEx.ConstraintName, constraintName, StringComparison.OrdinalIgnoreCase) ||
                   (pgEx.Message?.Contains(constraintName, StringComparison.OrdinalIgnoreCase) ?? false);
        }

        return false;
    }
}