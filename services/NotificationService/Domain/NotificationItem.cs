namespace NotificationService.Domain;
public class NotificationItem { public Guid Id { get; set; } public string Message { get; set; } = string.Empty; public bool IsRead { get; set; } public DateTime CreatedUtc { get; set; } = DateTime.UtcNow; }
