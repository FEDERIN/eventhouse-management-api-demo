using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;

namespace EventHouse.Management.Infrastructure.Persistence.Exceptions;

public static class DbUpdateExceptionExtensions
{
    public static bool IsUniqueViolation(this DbUpdateException ex)
    {
        if (ex.InnerException is SqliteException sqlite &&
            sqlite.SqliteErrorCode == 19)
            return true;

        return ex.InnerException?.Message
            .Contains("UNIQUE constraint failed", StringComparison.OrdinalIgnoreCase) == true;
    }
}
