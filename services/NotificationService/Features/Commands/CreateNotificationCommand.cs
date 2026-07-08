using MediatR;
using NotificationService.Data;
using NotificationService.Domain;
namespace NotificationService.Features.Commands;
public record CreateNotificationCommand(string Message) : IRequest<NotificationDto>;
public class CreateNotificationCommandHandler(AppDbContext db) : IRequestHandler<CreateNotificationCommand, NotificationDto>
{
    public async Task<NotificationDto> Handle(CreateNotificationCommand request, CancellationToken ct)
    {
        var item = new NotificationItem { Id = Guid.NewGuid(), Message = request.Message };
        db.Notifications.Add(item);
        await db.SaveChangesAsync(ct);
        return new NotificationDto(item.Id, item.Message, item.IsRead, item.CreatedUtc);
    }
}
