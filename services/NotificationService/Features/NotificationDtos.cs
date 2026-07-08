namespace NotificationService.Features;
public record NotificationDto(Guid Id, string Message, bool IsRead, DateTime CreatedUtc);
public record CreateNotificationRequest(string Message);
