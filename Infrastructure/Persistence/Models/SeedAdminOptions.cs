namespace Infrastructure.Persistence.Models;
public sealed class SeedAdminOptions
{
    public string Email { get; set; } = null!;
    public string UserName { get; set; } = null!;
    public string Password { get; set; } = null!;
    public string Role { get; set; } = null!;
}
