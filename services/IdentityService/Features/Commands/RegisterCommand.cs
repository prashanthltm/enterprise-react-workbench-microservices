using IdentityService.Data;
using IdentityService.Domain;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace IdentityService.Features.Commands;
public record RegisterCommand(string Name, string Email, string Password) : IRequest<AuthResponse>;
public class RegisterCommandHandler(AppDbContext db) : IRequestHandler<RegisterCommand, AuthResponse>
{
    public async Task<AuthResponse> Handle(RegisterCommand request, CancellationToken ct)
    {
        var user = await db.AppUsers.FirstOrDefaultAsync(x => x.Email == request.Email, ct);
        if (user is null)
        {
            user = new AppUser { Id = Guid.NewGuid(), Name = request.Name, Email = request.Email, PasswordHash = request.Password, RoleName = "User" };
            db.AppUsers.Add(user);
            await db.SaveChangesAsync(ct);
        }
        var token = Convert.ToBase64String(Guid.NewGuid().ToByteArray());
        return new AuthResponse(token, new AuthUserDto(user.Id, user.Name, user.Email, user.RoleName));
    }
}
