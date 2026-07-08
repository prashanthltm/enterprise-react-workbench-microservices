namespace DashboardService.Features;
public record TeamScoreDto(string Team, int Users, int AvgScore);
public record MetricItem(string Name, int Value);
public record DashboardDto(int TotalUsers, int ActiveUsers, int NotificationCount, List<TeamScoreDto> TeamScores, List<MetricItem> StatusSummary);
