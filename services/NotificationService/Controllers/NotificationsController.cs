using MediatR;
using Microsoft.AspNetCore.Mvc;
using NotificationService.Features;
using NotificationService.Features.Commands;
using NotificationService.Features.Queries;

namespace NotificationService.Controllers;
[ApiController]
[Route("api/[controller]")]
public class NotificationsController(IMediator mediator) : ControllerBase
{
    [HttpGet]
    public async Task<IActionResult> Get() => Ok(await mediator.Send(new GetNotificationsQuery()));
    [HttpPost]
    public async Task<IActionResult> Create(CreateNotificationRequest request) => Ok(await mediator.Send(new CreateNotificationCommand(request.Message)));
}
