namespace Bookify.Domain.Users;

public sealed class Role(int id, string name)
{
    public static readonly Role Registered = new(1, "Registered");
    public string Name { get; init; } = name;

    public int Id { get; init; } = id;
    
    public ICollection<User> Users { get; init; } = new List<User>();
    public ICollection<Permission> Permissions { get; init; } = new List<Permission>();
}