using Bookify.Domain.Users;

namespace Bookify.Infrastructure.Authorization;

public sealed class UserRolesResponse
{
    public Guid UserId { get; set; }
    
    public List<Role> Roles { get; set; } = [];
}