using DashboardService.Data;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace DashboardService.Features.Queries;
public record GetDashboardQuery : IRequest<DashboardDto>;
public class GetDashboardQueryHandler(AppDbContext db) : IRequestHandler<GetDashboardQuery, DashboardDto>
{
    public async Task<DashboardDto> Handle(GetDashboardQuery request, CancellationToken ct)
    {
        var users = await db.UserProfiles.ToListAsync(ct);
        var notificationCount = await db.Notifications.CountAsync(ct);
        var activeUsers = users.Count(x => x.Status == "Active");
        var teamScores = users.GroupBy(x => x.Team).Select(g => new TeamScoreDto(g.Key, g.Count(), (int)Math.Round(g.Average(x => x.Score)))).OrderBy(x => x.Team).ToList();
        return new DashboardDto(users.Count, activeUsers, notificationCount, teamScores, new List<MetricItem> { new("Active", activeUsers), new("Inactive", users.Count - activeUsers) });
    }
}
