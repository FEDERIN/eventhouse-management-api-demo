using FluentValidation;
using MediatR;
using EventHouse.Management.Application.Common.Behaviors;

namespace EventHouse.Management.Application.Tests.Common.Behaviors;

public class ValidationBehaviorTests
{
    // 1. Comando de prueba
    public record TestCommand(string Name) : IRequest<Unit>;

    // 2. Validador "Fake" para pruebas que DEBEN fallar
    public class FailingValidator : AbstractValidator<TestCommand>
    {
        public FailingValidator()
        {
            RuleFor(x => x.Name).NotEmpty().WithMessage("Name is required");
        }
    }

    // 3. Validador "Fake" para pruebas que DEBEN pasar
    public class PassingValidator : AbstractValidator<TestCommand>
    {
        public PassingValidator()
        {
            // Siempre pasa
        }
    }

    [Fact]
    public async Task Handle_ShouldThrowValidationException_WhenValidationFails()
    {
        // Arrange
        var validators = new List<IValidator<TestCommand>> { new FailingValidator() };
        var behavior = new ValidationBehavior<TestCommand, Unit>(validators);

        // Act
        await Assert.ThrowsAsync<FluentValidation.ValidationException>(() =>
            behavior.Handle(new TestCommand(""), (_) => Task.FromResult(Unit.Value), TestContext.Current.CancellationToken));
    }

    [Fact]
    public async Task Handle_ShouldProceed_WhenValidationSucceeds()
    {
        // Arrange
        var validators = new List<IValidator<TestCommand>> { new PassingValidator() };
        var behavior = new ValidationBehavior<TestCommand, Unit>(validators);
        var nextCalled = false;

        // Act
        await behavior.Handle(new TestCommand("Valid Name"), (_) =>
        {
            nextCalled = true;
            return Task.FromResult(Unit.Value);
        }, TestContext.Current.CancellationToken);

        // Assert
        Assert.True(nextCalled);
    }
}