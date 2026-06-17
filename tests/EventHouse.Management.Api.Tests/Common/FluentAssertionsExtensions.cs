using FluentAssertions;
using FluentAssertions.Equivalency;

namespace EventHouse.Management.Api.Tests.Common;

public static class FluentAssertionsExtensions
{
    /// <summary>
    /// Configures precision tolerance for both DateTime and DateTimeOffset
    /// to handle PostgreSQL's 6-decimal microsecond precision.
    /// </summary>
    public static EquivalencyOptions<TExpectation> WithPostgresPrecision<TExpectation>(
        this EquivalencyOptions<TExpectation> options)
    {
        // Handle DateTime
        options.Using<DateTime>(static ctx =>
            ctx.Subject.Should().BeCloseTo(ctx.Expectation, TimeSpan.FromMilliseconds(1)))
            .WhenTypeIs<DateTime>();

        // Handle DateTimeOffset
        options.Using<DateTimeOffset>(static ctx =>
            ctx.Subject.Should().BeCloseTo(ctx.Expectation, TimeSpan.FromMilliseconds(1)))
            .WhenTypeIs<DateTimeOffset>();

        return options;
    }
}