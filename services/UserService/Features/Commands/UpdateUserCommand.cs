using MediatR;
using Microsoft.EntityFrameworkCore;
using UserService.Data;

namespace UserService.Features.Commands;
public record UpdateUserCommand(Guid Id, string Name, string Role, string Email, string Team, string Status, int Score) : IRequest<UserDto?>;
public class UpdateUserCommandHandler(AppDbContext db) : IRequestHandler<UpdateUserCommand, UserDto?>
{
    public async Task<UserDto?> Handle(UpdateUserCommand request, CancellationToken ct)
    {
        var user = await db.UserProfiles.FirstOrDefaultAsync(x => x.Id == request.Id, ct);
        if (user is null) return null;
        user.Name = request.Name; user.Role = request.Role; user.Email = request.Email; user.Team = request.Team; user.Status = request.Status; user.Score = request.Score;
        await db.SaveChangesAsync(ct);
        return new UserDto(user.Id, user.Name, user.Role, user.Email, user.Status, user.Team, user.Score);
    }
}
