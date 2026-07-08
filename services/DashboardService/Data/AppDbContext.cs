using Microsoft.EntityFrameworkCore;
namespace DashboardService.Data;
public class AppDbContext(DbContextOptions<AppDbContext> options) : DbContext(options)
{
    public DbSet<UserProfileReadModel> UserProfiles => Set<UserProfileReadModel>();
    public DbSet<NotificationReadModel> Notifications => Set<NotificationReadModel>();
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<UserProfileReadModel>(e => { e.ToTable("user_profiles", "users"); e.HasKey(x => x.Id); e.Property(x => x.Id).HasColumnName("id"); e.Property(x => x.Status).HasColumnName("status"); e.Property(x => x.Team).HasColumnName("team"); e.Property(x => x.Score).HasColumnName("score"); });
        modelBuilder.Entity<NotificationReadModel>(e => { e.ToTable("notifications", "notification"); e.HasKey(x => x.Id); e.Property(x => x.Id).HasColumnName("id"); });
    }
}
public class UserProfileReadModel { public Guid Id { get; set; } public string Status { get; set; } = string.Empty; public string Team { get; set; } = string.Empty; public int Score { get; set; } }
public class NotificationReadModel { public Guid Id { get; set; } }
