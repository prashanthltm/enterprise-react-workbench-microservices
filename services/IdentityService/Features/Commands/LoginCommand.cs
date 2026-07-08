using MediatR;
using Microsoft.EntityFrameworkCore;
using IdentityService.Data;

namespace IdentityService.Features.Commands;
public record LoginCommand(string Email, string Password) : IRequest<AuthResponse?>;
public class LoginCommandHandler(AppDbContext db, IConfiguration configuration) : IRequestHandler<LoginCommand, AuthResponse?>
{
    public async Task<AuthResponse?> Handle(LoginCommand request, CancellationToken ct)
    {
        var user = await db.AppUsers.FirstOrDefaultAsync(x => x.Email == request.Email && x.PasswordHash == request.Password, ct);
        if (user is null) return null;
        var token = Convert.ToBase64String(Guid.NewGuid().ToByteArray());
        return new AuthResponse(token, new AuthUserDto(user.Id, user.Name, user.Email, user.RoleName));
    }
}
