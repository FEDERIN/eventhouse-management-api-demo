using FluentValidation;
using FluentValidation.TestHelper;
using System.Linq.Expressions;

namespace EventHouse.Management.Application.Tests.Commands;

public abstract class ValidatorTestBase<TCommand>(IValidator<TCommand> validator) where TCommand : class
{
    protected readonly IValidator<TCommand> Validator = validator;

    protected void ShouldHaveValidationError<TProperty>(TCommand command, Expression<Func<TCommand, TProperty>> propertyExpression, string expectedMessage)
    {
        
        Validator.TestValidate(command)
            .ShouldHaveValidationErrorFor(propertyExpression)
            .WithErrorMessage(expectedMessage);
    }

    protected void ShouldHaveValidationError<TProperty>(TCommand command, Expression<Func<TCommand, TProperty>> property)
    {
        Validator.TestValidate(command)
            .ShouldHaveValidationErrorFor(property);
    }

    protected void ShouldPassValidation(TCommand command)
    {
       Validator.TestValidate(command).ShouldNotHaveAnyValidationErrors();
    }
}