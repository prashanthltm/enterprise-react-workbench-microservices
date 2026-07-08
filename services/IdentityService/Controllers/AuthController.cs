using IdentityService.Features;
using IdentityService.Features.Commands;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace IdentityService.Controllers;
[ApiController]
[Route("api/[controller]")]
public class AuthController(IMediator mediator) : ControllerBase
{
    [HttpPost("login")]
    public async Task<IActionResult> Login(LoginRequest request)
    {
        var response = await mediator.Send(new LoginCommand(request.Email, request.Password));
        return response is null ? Unauthorized(new { message = "Invalid email or password" }) : Ok(response);
    }

    [HttpPost("register")]
    public async Task<IActionResult> Register(RegisterRequest request) => Ok(await mediator.Send(new RegisterCommand(request.Name, request.Email, request.Password)));
}
