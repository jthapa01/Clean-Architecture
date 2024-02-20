using Bookify.Application.Abstraction.Messaging;

namespace Bookify.Application.Users.LoginUser;

public record LoginUserCommand(string Email, string Password) 
    : ICommand<AccessTokenResponse>;