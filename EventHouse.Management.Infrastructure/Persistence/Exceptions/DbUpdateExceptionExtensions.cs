using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;

namespace EventHouse.Management.Infrastructure.Persistence.Exceptions;

public static class DbUpdateExceptionExtensions
{
    public static bool IsUniqueViolation(this DbUpdateException ex)
        => TryGetSqliteUniqueMessage(ex, out _);

    public static bool IsUniqueViolation(this DbUpdateException ex, params string[] containsAll)
    {
        if (!TryGetSqliteUniqueMessage(ex, out var msg))
            return false;

        return containsAll.All(t => msg.Contains(t, StringComparison.OrdinalIgnoreCase));
    }

    private static bool TryGetSqliteUniqueMessage(DbUpdateException ex, out string message)
    {
        message = ex.InnerException?.Message ?? string.Empty;

        if (ex.InnerException is SqliteException sqlite && sqlite.SqliteErrorCode == 19)
            return message.Length > 0;

        return message.Contains("UNIQUE constraint failed", StringComparison.OrdinalIgnoreCase);
    }
}
