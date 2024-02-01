using Bookify.Application.Abstraction.Behaviors;

namespace Bookify.Application.Exceptions;

public sealed class ValidationException(IEnumerable<ValidationError> errors) : Exception
{
    public IEnumerable<ValidationError> Errors { get; } = errors;
}