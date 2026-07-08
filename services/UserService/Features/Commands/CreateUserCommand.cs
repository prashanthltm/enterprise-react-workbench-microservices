using MediatR;
using UserService.Data;
using UserService.Domain;

namespace UserService.Features.Commands;
public record CreateUserCommand(string Name, string Role, string Email, string Team, string Status, int Score) : IRequest<UserDto>;
public class CreateUserCommandHandler(AppDbContext db) : IRequestHandler<CreateUserCommand, UserDto>
{
    public async Task<UserDto> Handle(CreateUserCommand request, CancellationToken ct)
    {
        var user = new UserProfile { Id = Guid.NewGuid(), Name = request.Name, Role = request.Role, Email = request.Email, Team = request.Team, Status = request.Status, Score = request.Score };
        db.UserProfiles.Add(user);
        await db.SaveChangesAsync(ct);
        return new UserDto(user.Id, user.Name, user.Role, user.Email, user.Status, user.Team, user.Score);
    }
}
