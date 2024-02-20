using Bookify.Application.Abstraction.Authentication;
using Bookify.Application.Abstraction.Messaging;
using Bookify.Domain.Abstractions;
using Bookify.Domain.Users;

namespace Bookify.Application.Users.RegisterUser;

public class RegisterUserCommandHandler(
    IAuthenticationService authenticationService,
    IUserRepository userRepository,
    IUnitOfWork unitOfWork)
    : ICommandHandler<RegisterUserCommand, Guid>
{
    public async Task<Result<Guid>> Handle(
        RegisterUserCommand request, 
        CancellationToken cancellationToken)
    {
        try
        {
            var user = User.Create(
                new FirstName(request.FirstName),
                new LastName(request.LastName),
                new Email(request.Email));
        
            var identityId = await authenticationService.RegisterAsync(
                user,
                request.Password,
                cancellationToken);
        
            user.SetIdentityId(identityId);
        
            var existingUser = await userRepository.GetByEmailAsync(request.Email);
        
            if (existingUser != null)
            {
                return Result.Failure<Guid>(UserErrors.EmailAlreadyExists);
            }
        
            userRepository.Add(user);
        
            await unitOfWork.SaveChangesAsync(cancellationToken);

            return user.Id;
        }
        catch (HttpRequestException)
        {
            return Result.Failure<Guid>(UserErrors.EmailAlreadyExists);
        }
    }
}