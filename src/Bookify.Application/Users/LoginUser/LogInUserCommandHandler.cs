using Bookify.Application.Abstraction.Authentication;
using Bookify.Application.Abstraction.Messaging;
using Bookify.Domain.Abstractions;
using Bookify.Domain.Users;

namespace Bookify.Application.Users.LoginUser;

public class LogInUserCommandHandler(IJwtService jwtService) : ICommandHandler<LoginUserCommand, AccessTokenResponse>
{
    public async Task<Result<AccessTokenResponse>> Handle(
        LoginUserCommand request, 
        CancellationToken cancellationToken)
    {
        var result = await jwtService.GetAccessTokenAsync(
            request.Email, 
            request.Password, 
            cancellationToken);
        
        return result.IsFailure ? 
            Result.Failure<AccessTokenResponse>(UserErrors.InvalidCredentials) 
            : new AccessTokenResponse(result.Value);
    }
}