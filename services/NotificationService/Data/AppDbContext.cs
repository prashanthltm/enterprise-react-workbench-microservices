using Microsoft.EntityFrameworkCore;
using NotificationService.Domain;
namespace NotificationService.Data;
public class AppDbContext(DbContextOptions<AppDbContext> options) : DbContext(options)
{
    public DbSet<NotificationItem> Notifications => Set<NotificationItem>();
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.HasDefaultSchema("notification");
        modelBuilder.Entity<NotificationItem>(e => { e.ToTable("notifications"); e.HasKey(x => x.Id); e.Property(x => x.Id).HasColumnName("id"); e.Property(x => x.Message).HasColumnName("message"); e.Property(x => x.IsRead).HasColumnName("is_read"); e.Property(x => x.CreatedUtc).HasColumnName("created_utc"); });
    }
}
