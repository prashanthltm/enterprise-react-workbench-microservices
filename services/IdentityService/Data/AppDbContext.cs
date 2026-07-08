using IdentityService.Domain;
using Microsoft.EntityFrameworkCore;

namespace IdentityService.Data;
public class AppDbContext(DbContextOptions<AppDbContext> options) : DbContext(options)
{
    public DbSet<AppUser> AppUsers => Set<AppUser>();
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.HasDefaultSchema("identity");
        modelBuilder.Entity<AppUser>(entity =>
        {
            entity.ToTable("app_users");
            entity.HasKey(x => x.Id);
            entity.Property(x => x.Id).HasColumnName("id");
            entity.Property(x => x.Name).HasColumnName("name").HasMaxLength(200);
            entity.Property(x => x.Email).HasColumnName("email").HasMaxLength(250);
            entity.Property(x => x.PasswordHash).HasColumnName("password_hash");
            entity.Property(x => x.RoleName).HasColumnName("role_name").HasMaxLength(100);
            entity.Property(x => x.CreatedUtc).HasColumnName("created_utc");
        });
    }
}
