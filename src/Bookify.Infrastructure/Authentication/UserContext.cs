using Bookify.Application.Abstraction.Authentication;
using Microsoft.AspNetCore.Http;

namespace Bookify.Infrastructure.Authentication;

public sealed class UserContext(IHttpContextAccessor httpContextAccessor) : IUserContext
{
    public Guid UserId => httpContextAccessor.HttpContext?.User.GetUserId() ??
                           throw new ApplicationException("User context is unavailable");
    
    public string IdentityId => httpContextAccessor.HttpContext?.User.GetIdentityId() ??
                                throw new ApplicationException("User context is unavailable");
}