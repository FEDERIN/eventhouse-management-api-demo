using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;

namespace EventHouse.Management.Infrastructure.Persistence.Exceptions;

public static class DbUpdateExceptionExtensions
{
    public static bool IsUniqueViolation(this DbUpdateException ex)
    {
        return (ex.InnerException is Microsoft.Data.Sqlite.SqliteException { SqliteErrorCode: 19 });
    }

    public static bool IsUniqueViolation(this DbUpdateException ex, string indexOrColumnName)
    {
        if (!ex.IsUniqueViolation()) return false;

        var message = ex.InnerException?.Message ?? string.Empty;

         return message.Contains(indexOrColumnName, StringComparison.OrdinalIgnoreCase);
    }
}