using Microsoft.AspNetCore.Authorization;

namespace Bookify.Infrastructure.Authorization;

public class PermissionRequirement(string permission) : IAuthorizationRequirement
{
    public string Permission { get; } = permission;
}