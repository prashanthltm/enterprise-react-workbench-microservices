using Microsoft.EntityFrameworkCore;
using UserService.Domain;

namespace UserService.Data;
public class AppDbContext(DbContextOptions<AppDbContext> options) : DbContext(options)
{
    public DbSet<UserProfile> UserProfiles => Set<UserProfile>();
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.HasDefaultSchema("users");
        modelBuilder.Entity<UserProfile>(entity =>
        {
            entity.ToTable("user_profiles");
            entity.HasKey(x => x.Id);
            entity.Property(x => x.Id).HasColumnName("id");
            entity.Property(x => x.IdentityUserId).HasColumnName("identity_user_id");
            entity.Property(x => x.Name).HasColumnName("name").HasMaxLength(200);
            entity.Property(x => x.Role).HasColumnName("role").HasMaxLength(100);
            entity.Property(x => x.Email).HasColumnName("email").HasMaxLength(250);
            entity.Property(x => x.Status).HasColumnName("status").HasMaxLength(50);
            entity.Property(x => x.Team).HasColumnName("team").HasMaxLength(100);
            entity.Property(x => x.Score).HasColumnName("score");
            entity.Property(x => x.CreatedUtc).HasColumnName("created_utc");
        });
    }
}
