namespace EventHouse.Management.Application.Common
{
    public enum UpdateResult
    {
        Success,
        NotFound,
        ValidationFailed, // 400
        Conflict,          // 409 (regla/unique/concurrency)
        InvalidState,
        NoChanges
    }
}
