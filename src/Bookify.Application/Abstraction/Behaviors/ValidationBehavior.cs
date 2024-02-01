using Bookify.Application.Abstraction.Messaging;
using FluentValidation;
using MediatR;

namespace Bookify.Application.Abstraction.Behaviors;

internal sealed class ValidationBehavior<TRequest, TResponse>
    (IEnumerable<IValidator<TRequest>> validators)
    : IPipelineBehavior<TRequest, TResponse>
    where TRequest : IBaseCommand
{
    public async Task<TResponse> Handle(
        TRequest request, 
        RequestHandlerDelegate<TResponse> next, 
        CancellationToken cancellationToken)
    {
        if (!validators.Any())
        {
            return await next();
        }
        
        var context = new ValidationContext<TRequest>(request);
        
        var validationResults = validators
            .Select(validator => validator.Validate(context))
            .Where(validationResult => validationResult.Errors.Count != 0)
            .SelectMany(result => result.Errors)
            .Select(validationFailure => new ValidationError(
                validationFailure.PropertyName, 
                validationFailure.ErrorMessage))
            .ToList();

        if (validationResults.Count != 0)
        {
            throw new Exceptions.ValidationException(validationResults);
        }

        return await next();
    }
}