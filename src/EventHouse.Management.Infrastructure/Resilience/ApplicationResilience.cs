using Core.Resilience.Abstractions;
using EventHouse.Management.Application.Common.Interfaces;
using EventHouse.Management.Infrastructure.Resilience.Exceptions;

namespace EventHouse.Management.Infrastructure.Resilience;

internal sealed class ApplicationResilience(
    IResiliencePipelineProvider pipelineProvider)
    : IApplicationResilience
{
    private readonly IResiliencePipeline _sqlPipeline =
        pipelineProvider.GetPipeline(PipelineType.Sql);

    public async Task ExecuteSqlAsync(
        Func<CancellationToken, Task> operation,
        CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(operation);

        try
        {
            await _sqlPipeline.ExecuteAsync(operation, cancellationToken);
        }
        catch (Exception ex)
        {
            throw PostgresExceptionMapper.Map(ex);
        }
    }

    public async Task<TResult> ExecuteSqlAsync<TResult>(
        Func<CancellationToken, Task<TResult>> operation,
        CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(operation);

        try
        {
            return await _sqlPipeline.ExecuteAsync(operation, cancellationToken);
        }
        catch (Exception ex)
        {
            throw PostgresExceptionMapper.Map(ex);
        }
    }
}