using MediatR;
using Microsoft.EntityFrameworkCore;
using UserService.Data;

namespace UserService.Features.Queries;
public record GetUsersQuery : IRequest<List<UserDto>>;
public class GetUsersQueryHandler(AppDbContext db) : IRequestHandler<GetUsersQuery, List<UserDto>>
{
    public async Task<List<UserDto>> Handle(GetUsersQuery request, CancellationToken ct) =>
        await db.UserProfiles.OrderBy(x => x.Name)
            .Select(x => new UserDto(x.Id, x.Name, x.Role, x.Email, x.Status, x.Team, x.Score))
            .ToListAsync(ct);
}
