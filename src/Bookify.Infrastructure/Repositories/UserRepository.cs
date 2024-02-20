using Bookify.Domain.Users;
using Microsoft.EntityFrameworkCore;

namespace Bookify.Infrastructure.Repositories;

internal sealed class UserRepository(ApplicationDbContext dbContext) 
    : Repository<User>(dbContext), IUserRepository
{
    public async Task<User?> GetByEmailAsync(string email)
    {
        var users = await DbContext.Set<User>()
            .AsNoTracking()
            .ToListAsync();

        return users.FirstOrDefault(user => user.Email.Value == email);
    }
}
