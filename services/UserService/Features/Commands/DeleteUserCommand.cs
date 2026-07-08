using MediatR;
using Microsoft.EntityFrameworkCore;
using UserService.Data;

namespace UserService.Features.Commands;
public record DeleteUserCommand(Guid Id) : IRequest<bool>;
public class DeleteUserCommandHandler(AppDbContext db) : IRequestHandler<DeleteUserCommand, bool>
{
    public async Task<bool> Handle(DeleteUserCommand request, CancellationToken ct)
    {
        var user = await db.UserProfiles.FirstOrDefaultAsync(x => x.Id == request.Id, ct);
        if (user is null) return false;
        db.UserProfiles.Remove(user);
        await db.SaveChangesAsync(ct);
        return true;
    }
}
