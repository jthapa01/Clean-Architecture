using Bookify.Application.Abstraction.Authentication;
using Bookify.Application.Abstraction.Data;
using Bookify.Application.Abstraction.Messaging;
using Bookify.Domain.Abstractions;
using Dapper;

namespace Bookify.Application.Users.GetLoggedInUser;

internal sealed class GetLoggedInUserQueryHandler(
    ISqlConnectionFactory sqlConnectionFactory, 
    IUserContext userContext) : IQueryHandler<GetLoggedInUserQuery, UserResponse>
{
    public async Task<Result<UserResponse>> Handle(GetLoggedInUserQuery request, 
        CancellationToken cancellationToken)
    {
        using var connection = sqlConnectionFactory.CreateConnection();
        
        const string sql = """
                           SELECT
                                id AS Id,
                                first_name AS FirstName,
                                last_name AS LastName,
                                email AS Email
                           FROM users
                           WHERE identity_id = @IdentityId
                           """;
        
        var user = await connection.QuerySingleAsync<UserResponse>(
            sql,
            new
            {
                userContext.IdentityId
            });

        return user;
    }
}