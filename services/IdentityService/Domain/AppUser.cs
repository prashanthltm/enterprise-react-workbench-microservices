namespace IdentityService.Domain;
public class AppUser
{
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string PasswordHash { get; set; } = string.Empty;
    public string RoleName { get; set; } = "User";
    public DateTime CreatedUtc { get; set; } = DateTime.UtcNow;
}
