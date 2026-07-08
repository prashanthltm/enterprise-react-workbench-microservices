namespace UserService.Domain;
public class UserProfile
{
    public Guid Id { get; set; }
    public Guid? IdentityUserId { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Role { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string Status { get; set; } = "Active";
    public string Team { get; set; } = string.Empty;
    public int Score { get; set; }
    public DateTime CreatedUtc { get; set; } = DateTime.UtcNow;
}
