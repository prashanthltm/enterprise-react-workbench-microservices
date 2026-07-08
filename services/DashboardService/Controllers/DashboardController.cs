using DashboardService.Features.Queries;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace DashboardService.Controllers;
[ApiController]
[Route("api/[controller]")]
public class DashboardController(IMediator mediator) : ControllerBase
{
    [HttpGet]
    public async Task<IActionResult> Get() => Ok(await mediator.Send(new GetDashboardQuery()));
}
