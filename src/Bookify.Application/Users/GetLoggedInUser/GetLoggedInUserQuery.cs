using Bookify.Application.Abstraction.Messaging;

namespace Bookify.Application.Users.GetLoggedInUser;

public sealed record GetLoggedInUserQuery : IQuery<UserResponse>;