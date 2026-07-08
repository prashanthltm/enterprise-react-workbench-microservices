using MediatR;
using Microsoft.EntityFrameworkCore;
using NotificationService.Data;
namespace NotificationService.Features.Queries;
public record GetNotificationsQuery : IRequest<List<NotificationDto>>;
public class GetNotificationsQueryHandler(AppDbContext db) : IRequestHandler<GetNotificationsQuery, List<NotificationDto>>
{
    public async Task<List<NotificationDto>> Handle(GetNotificationsQuery request, CancellationToken ct) => await db.Notifications.OrderByDescending(x => x.CreatedUtc).Select(x => new NotificationDto(x.Id, x.Message, x.IsRead, x.CreatedUtc)).ToListAsync(ct);
}
