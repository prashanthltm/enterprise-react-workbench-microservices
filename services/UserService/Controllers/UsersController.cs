using MediatR;
using Microsoft.AspNetCore.Mvc;
using UserService.Features;
using UserService.Features.Commands;
using UserService.Features.Queries;

namespace UserService.Controllers;
[ApiController]
[Route("api/[controller]")]
public class UsersController(IMediator mediator) : ControllerBase
{
    [HttpGet]
    public async Task<IActionResult> Get() => Ok(await mediator.Send(new GetUsersQuery()));

    [HttpPost]
    public async Task<IActionResult> Create(CreateUserRequest request)
    {
        var user = await mediator.Send(new CreateUserCommand(request.Name, request.Role, request.Email, request.Team, request.Status, request.Score));
        return Created($"/api/users/{user.Id}", user);
    }

    [HttpPut("{id:guid}")]
    public async Task<IActionResult> Update(Guid id, UpdateUserRequest request)
    {
        var user = await mediator.Send(new UpdateUserCommand(id, request.Name, request.Role, request.Email, request.Team, request.Status, request.Score));
        return user is null ? NotFound() : Ok(user);
    }

    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> Delete(Guid id) => await mediator.Send(new DeleteUserCommand(id)) ? NoContent() : NotFound();
}
