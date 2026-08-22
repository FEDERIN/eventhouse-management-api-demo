namespace EventHouse.Management.Application.Common.Interfaces;

public interface IApplicationResilience
{
    Task ExecuteSqlAsync(
        Func<CancellationToken, Task> operation,
        CancellationToken ct = default);

    Task<TResult> ExecuteSqlAsync<TResult>(
        Func<CancellationToken, Task<TResult>> operation,
        CancellationToken ct = default);
}