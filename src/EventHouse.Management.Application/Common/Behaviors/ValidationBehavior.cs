using FluentValidation;
using MediatR;
using FVValidationException = FluentValidation.ValidationException;

namespace EventHouse.Management.Application.Common.Behaviors;

public sealed class ValidationBehavior<TRequest, TResponse>(
    IEnumerable<IValidator<TRequest>> validators)
    : IPipelineBehavior<TRequest, TResponse>
    where TRequest : notnull
{
    public async Task<TResponse> Handle(
        TRequest request,
        RequestHandlerDelegate<TResponse> next,
        CancellationToken ct)
    {
        var validatorList = validators as IList<IValidator<TRequest>> ?? [.. validators];
        if (validatorList.Count == 0)
            return await next(ct);

        var context = new ValidationContext<TRequest>(request);
        var results = await Task.WhenAll(
            validatorList.Select(v => v.ValidateAsync(context, ct)));

        var failures = results
            .SelectMany(r => r.Errors)
            .Where(f => f is not null)
            .ToList();

        if (failures.Count != 0)
            throw new FVValidationException(failures);

        return await next(ct);
    }
}
