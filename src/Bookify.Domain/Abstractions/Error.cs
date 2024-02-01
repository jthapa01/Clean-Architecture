namespace Bookify.Domain.Abstractions;

public record Error(string Code, string Name)
{
    public static readonly Error None = new(string.Empty, string.Empty);
    
    public static readonly Error NullValue = new("Error.NullValue", "Value cannot be null");
}