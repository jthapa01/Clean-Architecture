using Bookify.Domain.Users;

namespace Bookify.Application.Abstraction.Authentication;

public interface IAuthenticationService
{
    Task<string> RegisterAsync(User user, string password, CancellationToken cancellationToken = default);   
}