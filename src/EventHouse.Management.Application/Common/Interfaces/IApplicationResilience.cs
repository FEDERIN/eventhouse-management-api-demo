namespace EventHouse.Management.Application.Common.Interfaces;

public interface IApplicationResilience
{
    Task ExecuteSqlAsync(
        Func<CancellationToken, Task> operation,
        CancellationToken cancellationToken = default);

    Task<TResult> ExecuteSqlAsync<TResult>(
        Func<CancellationToken, Task<TResult>> operation,
        CancellationToken cancellationToken = default);
}