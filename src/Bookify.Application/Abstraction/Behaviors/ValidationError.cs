namespace Bookify.Application.Abstraction.Behaviors;

public sealed record ValidationError(string PropertyName, string ErrorMessage);