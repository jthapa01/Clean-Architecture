using Bookify.Domain.Abstractions;

namespace Bookify.Domain.Users;

public static class UserErrors
{
    public static Error NotFound => new(
        "User not found", 
        "The user with the given id was not found.");
    
    public static Error InvalidCredentials => new(
        "Invalid credentials", 
        "The provided credentials were invalid.");
    
    public static Error EmailAlreadyExists => new(
        "Email already exists", 
        "User with the given email already exists.");
}